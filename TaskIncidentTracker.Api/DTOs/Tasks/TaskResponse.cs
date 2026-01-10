using TaskIncidentTracker.Api.DTOs.Auth;
using TaskIncidentTracker.Api.Models;
using TaskStatus = TaskIncidentTracker.Api.Models.TaskStatus;

namespace TaskIncidentTracker.Api.DTOs.Tasks
{
    public class TaskResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }

        public UserResponse CreatedBy { get; set; }
        public List<UserResponse> AssignedTo { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

}
