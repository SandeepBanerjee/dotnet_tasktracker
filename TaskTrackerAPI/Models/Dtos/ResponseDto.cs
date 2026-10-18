namespace TaskTrackerAPI.Models.Dtos
{
    public class ResponseDto
    {
        public object? Result { get; set; }
        public bool IsSuccess { get; set; } = false;
        public string DisplayMessage { get; set; } = string.Empty;
    }
}
