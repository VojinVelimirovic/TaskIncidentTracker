namespace TaskIncidentTracker.Api.DTOs.Tasks
{
    public class TaskAssignmentRequest
    {
        public int TaskId { get; set; } 
        public List<int> UserIds { get; set; }
    }
}
