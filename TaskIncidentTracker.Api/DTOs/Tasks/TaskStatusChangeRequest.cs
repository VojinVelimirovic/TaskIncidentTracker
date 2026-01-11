using System.ComponentModel.DataAnnotations;
using TaskIncidentTracker.Api.Models;

namespace TaskIncidentTracker.Api.DTOs.Tasks
{
    public class TaskStatusChangeRequest
    {
        [Required]
        public int TaskId { get; set; }
        [Required]
        public Models.TaskStatus Status { get; set; }
    }
}
