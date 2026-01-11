using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using TaskIncidentTracker.Api.DTOs.Tasks;
using TaskIncidentTracker.Api.Services.Interfaces;

namespace TaskIncidentTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> Create(TaskCreationRequest task)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var createdTask = await _taskService.CreateTask(id, task);
            if (createdTask == null)
            {
                return BadRequest();
            }
            return Ok(new { message = "Task created successfully.", data = createdTask });
        }

        [HttpPost("assign")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Assign(TaskAssignmentRequest req)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var assignedTask = await _taskService.AssignTask(id, req);
            if (assignedTask == null)
            {
                return BadRequest();
            }
            return Ok(new { message = "Task assigned successfully" });
        }

        [HttpPost("change-status")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ChangeStatus(TaskStatusChangeRequest req)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var changedTask = await _taskService.ChangeTaskStatus(id, req);
            if (changedTask == null)
            {
                return BadRequest();
            }
            return Ok(new { message = $"Task status changed to {req.Status.ToString()}" });

        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> GetUserTasksPaged(string id, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var pagedTasks = await _taskService.GetUserTasks(id, page, pageSize);

            if (!pagedTasks.Items.Any())
            {
                return Ok(new { message = $"There are currently no tasks assigned to user {id} to be fetched."});
            }

            return Ok(new
            {
                message = $"Fetching tasks assigned to user {id} successful.",
                data = pagedTasks.Items,
                page = pagedTasks.Page,
                pageSize = pagedTasks.PageSize,
                totalCount = pagedTasks.TotalCount
            });
        }

        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetMyTasksPaged([FromQuery] int page = 1,[FromQuery] int pageSize = 20)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var pagedTasks = await _taskService.GetUserTasks(id, page, pageSize);

            if (!pagedTasks.Items.Any())
            {
                return Ok(new { message = $"There are currently no tasks assigned to you to be fetched."});
            }

            return Ok(new
            {
                message = $"Fetching tasks assigned to you successful.",
                data = pagedTasks.Items,
                page = pagedTasks.Page,
                pageSize = pagedTasks.PageSize,
                totalCount = pagedTasks.TotalCount
            });
        }


        [HttpGet]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> GetAllTasks([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var pagedTasks = await _taskService.GetAllTasks(page, pageSize);
            return Ok(new
            {
                message = $"Fetching tasks successful.",
                data = pagedTasks.Items,
                page = pagedTasks.Page,
                pageSize = pagedTasks.PageSize,
                totalCount = pagedTasks.TotalCount
            });
        }

    }
}
