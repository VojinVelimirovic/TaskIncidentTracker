using TaskIncidentTracker.Api.DTOs.Tasks;
using TaskIncidentTracker.Api.Models;

namespace TaskIncidentTracker.Api.Services.Interfaces
{
    public interface ITaskService
    {
        Task<TaskResponse> CreateTask(string creatorId, TaskCreationRequest taskRequest);
        Task<TaskResponse?> AssignTask(string managerId, TaskAssignmentRequest req);
        Task<TaskResponse?> ChangeTaskStatus(string managerId, TaskStatusChangeRequest req);
        Task<List<TaskResponse>> GetAllTasks();
        Task<List<TaskResponse>> GetUserTasks(string userId);
    }
}
