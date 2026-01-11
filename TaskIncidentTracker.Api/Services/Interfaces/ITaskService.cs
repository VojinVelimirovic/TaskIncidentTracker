using TaskIncidentTracker.Api.DTOs.Tasks;
using TaskIncidentTracker.Api.Models;
using TaskIncidentTracker.Api.Common;

namespace TaskIncidentTracker.Api.Services.Interfaces
{
    public interface ITaskService
    {
        Task<TaskResponse> CreateTask(string creatorId, TaskCreationRequest taskRequest);
        Task<TaskResponse?> AssignTask(string managerId, TaskAssignmentRequest req);
        Task<TaskResponse?> ChangeTaskStatus(string managerId, TaskStatusChangeRequest req);
        Task<PagedResult<TaskResponse>> GetAllTasks(int page, int pageSize);
        Task<PagedResult<TaskResponse>> GetUserTasks(string userId, int page, int pageSize);
    }
}
