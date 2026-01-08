using TaskIncidentTracker.Api.DTOs.Tasks;
using TaskIncidentTracker.Api.Models;

namespace TaskIncidentTracker.Api.Services.Interfaces
{
    public interface ITaskService
    {
        Task<TaskItem> CreateTask(string creatorId, TaskCreationRequest taskRequest);
        Task<TaskItem?> AssignTask(TaskAssignmentRequest req);
        Task<TaskItem?> ChangeTaskStatus(TaskStatusChangeRequest req);
    }
}
