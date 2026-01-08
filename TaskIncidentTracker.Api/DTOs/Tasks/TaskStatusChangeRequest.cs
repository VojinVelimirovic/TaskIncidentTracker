using TaskIncidentTracker.Api.Models;

namespace TaskIncidentTracker.Api.DTOs.Tasks
{
    public class TaskStatusChangeRequest
    {
        public int TaskId { get; set; }
        public Models.TaskStatus Status { get; set; }
    }
}
