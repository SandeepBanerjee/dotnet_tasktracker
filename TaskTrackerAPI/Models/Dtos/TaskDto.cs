using System.ComponentModel.DataAnnotations;

namespace TaskTrackerAPI.Models.Dtos
{
    public class TaskRequestDto
    {
        [Required]
        public string Title { get; set; }
        public string UserId { get; set; }
        public string TaskDescription { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }        
        public DateTime UpdatedAt { get; set; }        
        public bool isCompleted { get; set; } = false;
    }

    public class TaskRsponseDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string TaskDescription { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }        
        public DateTime? UpdatedAt { get; set; }        
        public bool isCompleted { get; set; } = false;
    }
}
