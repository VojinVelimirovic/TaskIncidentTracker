using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskIncidentTracker.Api.Data;
using TaskIncidentTracker.Api.DTOs.Tasks;
using TaskIncidentTracker.Api.DTOs.Auth;
using TaskIncidentTracker.Api.Models;
using TaskIncidentTracker.Api.Services.Interfaces;
using TaskIncidentTracker.Api.Mappers;
using TaskIncidentTracker.Api.Common;

namespace TaskIncidentTracker.Api.Services.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TaskService> _logger;
        private readonly ITaskMapper _taskMapper;

        public TaskService(ApplicationDbContext context, ILogger<TaskService> logger, ITaskMapper taskMapper)
        {
            _context = context;
            _logger = logger;
            _taskMapper = taskMapper;
        }

        public async Task<TaskResponse?> AssignTask(string managerId, TaskAssignmentRequest req)
        {
            var task = await _context.Tasks
                .Include(t => t.AssignedTo)
                .FirstOrDefaultAsync(t => t.Id == req.TaskId);
            if (task == null)
            {
                return null;
            }
            foreach (int userId in req.UserIds)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    continue;
                }
                if (!task.AssignedTo.Any(u => u.Id == userId))
                {
                    task.AssignedTo.Add(user);
                }
            }
            task.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            _logger.LogInformation(
                "Manager {ManagerId} assigned task {TaskId} to users {UserIds}",
                managerId,
                task.Id,
                req.UserIds
            );

            return _taskMapper.ToResponse(task);
        }

        public async Task<TaskResponse?> ChangeTaskStatus(string managerId, TaskStatusChangeRequest req)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == req.TaskId);
            if (task == null)
            {
                return null;
            }
            task.Status = req.Status;
            task.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Manager {managerId} has changed task {task.Id}'s status to {task.Status.ToString()}");
            return _taskMapper.ToResponse(task);
        }

        public async Task<TaskResponse> CreateTask(string creatorId, TaskCreationRequest taskRequest)
        {
            var task = new TaskItem
            {
                Title = taskRequest.Title,
                Description = taskRequest.Description,
                Status = Models.TaskStatus.OPEN,
                Priority = taskRequest.Priority,
                CreatedById = Int32.Parse(creatorId),
                CreatedAt = DateTime.UtcNow
            };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"User {creatorId} has created task {task.Id} - {task.Title}");
            var createdTask = await _context.Tasks
            .Include(t => t.CreatedBy)
            .Include(t => t.AssignedTo)
            .FirstOrDefaultAsync(t => t.Id == task.Id);
            return _taskMapper.ToResponse(createdTask!);
        }

        public async Task<PagedResult<TaskResponse>> GetAllTasks(int page, int pageSize)
        {
            var query = _context.Tasks
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .OrderByDescending(t => t.UpdatedAt ?? t.CreatedAt);

            var totalCount = await query.CountAsync();

            var tasks = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<TaskResponse>
            {
                Items = tasks.Select(t => _taskMapper.ToResponse(t)).ToList(),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<PagedResult<TaskResponse>> GetUserTasks(string userId, int page, int pageSize)
        {
            if (!int.TryParse(userId, out var parsedUserId))
            {
                return new PagedResult<TaskResponse>
                {
                    Items = new List<TaskResponse>(),
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = 0
                };
            }

            var query = _context.Tasks
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .Where(t => t.AssignedTo.Any(u => u.Id == parsedUserId));

            var totalCount = await query.CountAsync();

            var tasks = await query
                .OrderByDescending(t => t.UpdatedAt ?? t.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<TaskResponse>
            {
                Items = tasks.Select(t => _taskMapper.ToResponse(t)).ToList(),
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

    }
}
