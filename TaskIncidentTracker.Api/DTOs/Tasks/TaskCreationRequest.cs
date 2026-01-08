using TaskIncidentTracker.Api.Models;

namespace TaskIncidentTracker.Api.DTOs.Tasks
{
    public class TaskCreationRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskPriority Priority { get; set; }
    }
}
