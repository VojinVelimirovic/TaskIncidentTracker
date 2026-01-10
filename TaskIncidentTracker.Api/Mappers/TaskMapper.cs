using TaskIncidentTracker.Api.DTOs.Auth;
using TaskIncidentTracker.Api.DTOs.Tasks;
using TaskIncidentTracker.Api.Models;

namespace TaskIncidentTracker.Api.Mappers
{
    public interface ITaskMapper
    {
        TaskResponse ToResponse(TaskItem task);
    }

    public class TaskMapper : ITaskMapper
    {
        public TaskResponse ToResponse(TaskItem task)
        {
            return new TaskResponse
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                Priority = task.Priority,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt,

                CreatedBy = new UserResponse
                {
                    Id = task.CreatedBy.Id,
                    Username = task.CreatedBy.Username,
                    Role = task.CreatedBy.Role
                },

                AssignedTo = task.AssignedTo
                    .Select(u => new UserResponse
                    {
                        Id = u.Id,
                        Username = u.Username,
                        Role = u.Role
                    })
                    .ToList()
            };
        }
    }
}
