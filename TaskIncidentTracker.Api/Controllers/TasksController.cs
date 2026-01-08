using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult> Create(TaskCreationRequest task)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var createdTask = await _taskService.CreateTask(id, task);
            if (createdTask == null)
            {
                return BadRequest();
            }
            return Ok(new { createdTask });
        }

        [HttpPost("assign")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> Assign(TaskAssignmentRequest req)
        {
            var assignedTask = await _taskService.AssignTask(req);
            if (assignedTask == null)
            {
                return BadRequest();
            }
            return Ok(new { message = "Task assigned successfully" });
        }

        [HttpPost("change-status")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> ChangeStatus(TaskStatusChangeRequest req)
        {
            var changedTask = await _taskService.ChangeTaskStatus(req);
            if (changedTask == null)
            {
                return BadRequest();
            }
            return Ok(new { message = $"Task status changed to {req.Status}" });

        }
    }
}
