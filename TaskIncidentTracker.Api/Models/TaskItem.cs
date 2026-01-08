
namespace TaskIncidentTracker.Api.Models
{
    public enum TaskStatus
    {
        OPEN, IN_PROGRESS, BLOCKED, DONE
    }
    public enum TaskPriority
    {
        Low, Medium, High, Critical
    }
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public User CreatedBy { get; set; }
        public int CreatedById { get; set; }
        public List<User> AssignedTo { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
