using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskTrackerAPI.Data;
using TaskTrackerAPI.Models.Dtos;
using TaskTrackerAPI.Services.Interfaces;

namespace TaskTrackerAPI.Controllers
{
    [Route("api/task")]
    [Authorize]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskTrackerService _taskService;
        private readonly ResponseDto _response;

        public TaskController(ITaskTrackerService taskService)
        {
            _taskService = taskService;
            _response = new ResponseDto();
        }


        [HttpGet]
        public async Task<IActionResult> AllTasks()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            var allTasks = await _taskService.GetAllTask(userId);

            if (allTasks == null)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "No tasks found";
                return NotFound( _response );
            }

            _response.IsSuccess = true;
            _response.Result = allTasks;

            return Ok(_response);
        }

        [HttpPost("{id?}")]
        public async Task<IActionResult> SaveTask([FromBody] TaskRequestDto request, int? taskId)
        {
            var saveResponse = await _taskService.SaveTask(request, taskId);

            if (saveResponse == null)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "task data not saved";

                return BadRequest(_response);
            }

            _response.IsSuccess = true;
            _response.Result = saveResponse;

            return Ok(_response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> TaskDetail(int taskId)
        {
            var taskDetails = await _taskService.GetTaskDetails(taskId);
            if (taskDetails == null)
            {
                _response.IsSuccess =false;
                _response.DisplayMessage = $"Task with Id: {taskId} not found";

                return NotFound(_response);
            }
            _response.IsSuccess = true;
            _response.Result = taskDetails;

            return Ok(_response);
        }

        [HttpPatch("{id}/complete")]
        public async Task<IActionResult> MarkComplete(int taskId)
        {
            var taskDetails = await _taskService.CompleteTask(taskId);
            if (taskDetails == null)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = $"Task with Id: {taskId} not found";

                return NotFound(_response);
            }
            _response.IsSuccess = true;
            _response.Result = taskDetails;

            return Ok(_response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            var deleteResponse = await _taskService.DeleteTask(taskId);
            if (deleteResponse == null)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = $"Task with Id: {taskId} not found";

                return NotFound(_response);
            }

            return NoContent();
        }
    }
}
