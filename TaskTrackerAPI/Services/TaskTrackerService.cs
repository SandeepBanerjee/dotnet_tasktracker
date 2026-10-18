using Microsoft.EntityFrameworkCore;
using TaskTrackerAPI.Data;
using TaskTrackerAPI.Models;
using TaskTrackerAPI.Models.Dtos;
using TaskTrackerAPI.Services.Interfaces;

namespace TaskTrackerAPI.Services
{
    public class TaskTrackerService : ITaskTrackerService
    {
        private readonly AppDbContext _appDbContext;

        public TaskTrackerService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<TaskRsponseDto> CompleteTask(int taskId)
        {
            var taskDetails = await _appDbContext.TaskTracker.FirstOrDefaultAsync(task => task.Id == taskId);

            if (taskDetails == null)
            {
                return null;
            }

            taskDetails.isCompleted = true;

            await _appDbContext.SaveChangesAsync();

            return new TaskRsponseDto
            {
                Id = taskDetails.Id,
                UserId = taskDetails.UserId,
                Title = taskDetails.Title,
                TaskDescription = taskDetails.TaskDescription,
                isCompleted = taskDetails.isCompleted
            };
        }

        public async Task<bool?> DeleteTask(int taskId)
        {
            var taskDetails = await _appDbContext.TaskTracker.FirstOrDefaultAsync(task => task.Id == taskId);

            if (taskDetails == null)
            {
                return null;
            }

            var deletedRows = await _appDbContext.TaskTracker.Where(task => task.Id == taskId).ExecuteDeleteAsync();

            if (deletedRows == 0)
            {
                return null;
            }
            return true;
        }

        public async Task<IEnumerable<TaskRsponseDto>> GetAllTask(string userId)
        {
            var TaskList = await _appDbContext.TaskTracker
                .AsNoTracking()
                .Where(task => task.UserId == userId)
                .Select(task => new TaskRsponseDto
            {
                Id = task.Id,
                Title = task.Title,
                TaskDescription = task.TaskDescription,
                UserId = task.UserId,
                isCompleted = task.isCompleted
            }).ToListAsync();

            return TaskList;
        }

        public async Task<TaskRsponseDto?> GetTaskDetails(int taskId)
        {
            var taskDetails = await _appDbContext.TaskTracker.FirstOrDefaultAsync( task => task.Id == taskId );

            if (taskDetails == null)
            {
                return null;
            }

            return new TaskRsponseDto
            {
                Id = taskDetails.Id,
                UserId = taskDetails.UserId,
                Title = taskDetails.Title,
                TaskDescription = taskDetails.TaskDescription,
                isCompleted = taskDetails.isCompleted
            };
        }

        public async Task<TaskRsponseDto> SaveTask(TaskRequestDto request, int? taskId)
        {
            TaskTracker taskTracker;
            var TaskTrackerId = 0;

            if (taskId == null)
            {
                //save tasks

                taskTracker = new TaskTracker
                {
                    Title = request.Title,
                    UserId = request.UserId,
                    TaskDescription = request.TaskDescription,
                    isCompleted = false,
                    CreatedAt = DateTime.UtcNow,
                };

                var saveTasks = await _appDbContext.TaskTracker.AddAsync(taskTracker);
                

            }
            else
            {
                //update tasks
                var existTask = await _appDbContext.TaskTracker.FirstOrDefaultAsync( task => task.Id == taskId );

                if (existTask == null)
                {
                    return null;
                }

                existTask.Title = request.Title;
                existTask.TaskDescription = request.TaskDescription;
                existTask.isCompleted = request.isCompleted;
                existTask.UpdatedAt = DateTime.UtcNow;
                taskTracker = existTask;

            }

            await _appDbContext.SaveChangesAsync();

            return new TaskRsponseDto
            {
                Id = taskTracker.Id,
                Title = taskTracker.Title,
                TaskDescription = taskTracker.TaskDescription,
                CreatedAt = taskTracker.CreatedAt,
                UpdatedAt = taskTracker.UpdatedAt,
                isCompleted = taskTracker.isCompleted
            };

        }
    }
}
