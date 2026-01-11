using System.ComponentModel.DataAnnotations;
using TaskIncidentTracker.Api.Models;

namespace TaskIncidentTracker.Api.DTOs.Tasks
{
    public class TaskCreationRequest
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public TaskPriority Priority { get; set; }
    }
}
