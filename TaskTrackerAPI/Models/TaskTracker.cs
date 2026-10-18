namespace TaskTrackerAPI.Models
{
    public class TaskTracker
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string TaskDescription { get; set; } = string.Empty;
        public bool isCompleted { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
