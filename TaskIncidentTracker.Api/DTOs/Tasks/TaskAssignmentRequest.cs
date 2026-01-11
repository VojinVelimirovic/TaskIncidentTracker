using System.ComponentModel.DataAnnotations;

namespace TaskIncidentTracker.Api.DTOs.Tasks
{
    public class TaskAssignmentRequest
    {
        [Required]
        public int TaskId { get; set; }
        [Required]
        public List<int> UserIds { get; set; }
    }
}
