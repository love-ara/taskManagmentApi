using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.Models.Dtos.Responses;
using TaskManagementAPI.Models.Dtos.Requests;
using TaskManagementAPI.Models.Entities;
using TaskManagementAPI.Services.Interfaces;

namespace TaskManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserTasksController : ControllerBase
    {
        private readonly IUserTasksService _taskService;
        private readonly ILogger<UserTasksController> _logger;

        public UserTasksController(IUserTasksService taskService, ILogger<UserTasksController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserTaskDto>>>> GetUserAllTasks()
        {
            try
            {
                var tasks = await _taskService.GetAllUserTasksAsync();
                return Ok(ApiResponse<IEnumerable<UserTaskDto>>.SuccessResponse(tasks, "Tasks retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all tasks");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<IEnumerable<UserTaskDto>>.FailureResponse("An error occurred while retrieving tasks. Please try again later."));
            }
        }

        [HttpGet("byId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<UserTaskDto>>> GetUserTaskById([FromQuery] Guid id)
        {
            try
            {
                var task = await _taskService.GetUserTaskByIdAsync(id);
                if (task == null)
                    return NotFound(ApiResponse<UserTaskDto>.FailureResponse($"Task with ID {id} not found"));

                return Ok(ApiResponse<UserTaskDto>.SuccessResponse(task, "Task retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving task with ID {TaskId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<UserTaskDto>.FailureResponse("An error occurred while retrieving the task. Please try again later."));
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<UserTaskDto>>> CreateUserTask(CreateUserTaskRequest createRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<UserTaskDto>.FailureResponse("Invalid task data"));

                var createdTask = await _taskService.CreateUserTaskAsync(createRequest);
                var response = ApiResponse<UserTaskDto>.SuccessResponse(createdTask, "Task created successfully");

                return CreatedAtAction(nameof(GetUserTaskById), new { id = createdTask.Id }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new task");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<UserTaskDto>.FailureResponse("An error occurred while creating the task. Please try again later."));
            }
        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<UserTaskDto>>> UpdateUserTask([FromQuery] Guid id, UpdateUserTaskRequest updateRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<UserTaskDto>.FailureResponse("Invalid task data"));

                var updatedTask = await _taskService.UpdateUserTaskAsync(id, updateRequest);
                if (updatedTask == null)
                    return NotFound(ApiResponse<UserTaskDto>.FailureResponse($"Task with ID {id} not found"));

                return Ok(ApiResponse<UserTaskDto>.SuccessResponse(updatedTask, "Task updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating task with ID {TaskId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<UserTaskDto>.FailureResponse("An error occurred while updating the task. Please try again later."));
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteUserTask([FromQuery] Guid id)
        {
            try
            {
                var result = await _taskService.DeleteUserTaskAsync(id);
                if (!result)
                    return NotFound(ApiResponse<bool>.FailureResponse($"Task with ID {id} not found"));

                return Ok(ApiResponse<bool>.SuccessResponse(true, "Task deleted successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting task with ID {TaskId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<bool>.FailureResponse("An error occurred while deleting the task. Please try again later."));
            }
        }

        [HttpGet("priority/{priority}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserTaskDto>>>> GetUserTasksByPriority( Priority priority)
        {
            try
            {
                var tasks = await _taskService.GetUserTasksByPriorityAsync(priority);
                return Ok(ApiResponse<IEnumerable<UserTaskDto>>.SuccessResponse(tasks, $"Tasks with priority {priority} retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving tasks with priority {Priority}", priority);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<IEnumerable<UserTaskDto>>.FailureResponse("An error occurred while retrieving tasks. Please try again later."));
            }
        }
    }
}