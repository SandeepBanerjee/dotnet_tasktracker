using TaskTrackerAPI.Models.Dtos;

namespace TaskTrackerAPI.Services.Interfaces
{
    public interface ITaskTrackerService
    {
        Task<IEnumerable<TaskRsponseDto>> GetAllTask(string userId);
        Task<TaskRsponseDto?> GetTaskDetails(int taskId);
        Task<TaskRsponseDto> SaveTask( TaskRequestDto request, int? taskId );
        Task<bool?> DeleteTask(int taskId);
        Task<TaskRsponseDto> CompleteTask(int taskId);
    }
}
