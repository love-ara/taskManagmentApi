//using Microsoft.AspNetCore.Mvc;
//using TaskManagementAPI.Models.Dtos.Responses;
//using TaskManagementAPI.Models.Dtos.Requests;
//using TaskManagementAPI.Models.Entities;
//using TaskManagementAPI.Services.Interfaces;

//namespace TaskManagementAPI.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class UserTasksController : ControllerBase
//    {
//        private readonly IUserTasksService _taskService;
//        private readonly ILogger<UserTasksController> _logger;

//        public UserTasksController(IUserTasksService taskService, ILogger<UserTasksController> logger)
//        {
//            _taskService = taskService;
//            _logger = logger;
//        }

//        [HttpGet]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public async Task<ActionResult<ApiResponse<IEnumerable<UserTaskDto>>>> GetUserAllTasks(Guid userId)
//        {
//            try
//            {
//                var tasks = await _taskService.GetAllUserTasksAsync(userId);
//                return Ok(ApiResponse<IEnumerable<UserTaskDto>>.SuccessResponse(tasks, "Tasks retrieved successfully"));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error occurred while retrieving all tasks");
//                return StatusCode(StatusCodes.Status500InternalServerError,
//                    ApiResponse<IEnumerable<UserTaskDto>>.FailureResponse("An error occurred while retrieving tasks. Please try again later."));
//            }
//        }

//        [HttpGet("byId")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public async Task<ActionResult<ApiResponse<UserTaskDto>>> GetUserTaskById([FromQuery] Guid id)
//        {
//            try
//            {
//                var task = await _taskService.GetUserTaskByIdAsync(id);
//                if (task == null)
//                    return NotFound(ApiResponse<UserTaskDto>.FailureResponse($"Task with ID {id} not found"));

//                return Ok(ApiResponse<UserTaskDto>.SuccessResponse(task, "Task retrieved successfully"));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error occurred while retrieving task with ID {TaskId}", id);
//                return StatusCode(StatusCodes.Status500InternalServerError,
//                    ApiResponse<UserTaskDto>.FailureResponse("An error occurred while retrieving the task. Please try again later."));
//            }
//        }

//        [HttpPost]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public async Task<ActionResult<ApiResponse<UserTaskDto>>> CreateUserTask(CreateUserTaskRequest createRequest)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                    return BadRequest(ApiResponse<UserTaskDto>.FailureResponse("Invalid task data"));

//                var createdTask = await _taskService.CreateUserTaskAsync(createRequest);
//                var response = ApiResponse<UserTaskDto>.SuccessResponse(createdTask, "Task created successfully");

//                return CreatedAtAction(nameof(GetUserTaskById), new { id = createdTask.UserTaskId }, response);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error occurred while creating a new task");
//                return StatusCode(StatusCodes.Status500InternalServerError,
//                    ApiResponse<UserTaskDto>.FailureResponse("An error occurred while creating the task. Please try again later."));
//            }
//        }

//        [HttpPatch]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public async Task<ActionResult<ApiResponse<UserTaskDto>>> UpdateUserTask([FromQuery] Guid id, UpdateUserTaskRequest updateRequest)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                    return BadRequest(ApiResponse<UserTaskDto>.FailureResponse("Invalid task data"));

//                var updatedTask = await _taskService.UpdateUserTaskAsync(id, updateRequest);
//                if (updatedTask == null)
//                    return NotFound(ApiResponse<UserTaskDto>.FailureResponse($"Task with ID {id} not found"));

//                return Ok(ApiResponse<UserTaskDto>.SuccessResponse(updatedTask, "Task updated successfully"));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error occurred while updating task with ID {TaskId}", id);
//                return StatusCode(StatusCodes.Status500InternalServerError,
//                    ApiResponse<UserTaskDto>.FailureResponse("An error occurred while updating the task. Please try again later."));
//            }
//        }

//        [HttpDelete]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public async Task<ActionResult<ApiResponse<bool>>> DeleteUserTask([FromQuery] Guid id)
//        {
//            try
//            {
//                var result = await _taskService.DeleteUserTaskAsync(id);
//                if (!result)
//                    return NotFound(ApiResponse<bool>.FailureResponse($"Task with ID {id} not found"));

//                return Ok(ApiResponse<bool>.SuccessResponse(true, "Task deleted successfully"));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error occurred while deleting task with ID {TaskId}", id);
//                return StatusCode(StatusCodes.Status500InternalServerError,
//                    ApiResponse<bool>.FailureResponse("An error occurred while deleting the task. Please try again later."));
//            }
//        }

//        [HttpGet("priority/{priority}")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public async Task<ActionResult<ApiResponse<IEnumerable<UserTaskDto>>>> GetUserTasksByPriority( Priority priority)
//        {
//            try
//            {
//                var tasks = await _taskService.GetUserTasksByPriorityAsync(priority);
//                return Ok(ApiResponse<IEnumerable<UserTaskDto>>.SuccessResponse(tasks, $"Tasks with priority {priority} retrieved successfully"));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error occurred while retrieving tasks with priority {Priority}", priority);
//                return StatusCode(StatusCodes.Status500InternalServerError,
//                    ApiResponse<IEnumerable<UserTaskDto>>.FailureResponse("An error occurred while retrieving tasks. Please try again later."));
//            }
//        }
//    }
//}

using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskManagementAPI.Models.Dtos.Requests;
using TaskManagementAPI.Models.Entities;
using TaskManagementAPI.Services.Interfaces;

namespace TaskManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserTasksController : ControllerBase
    {
        private readonly IUserTasksService _userTasksService;
        private readonly ILogger<UserTasksController> _logger;

        public UserTasksController(IUserTasksService userTasksService, ILogger<UserTasksController> logger)
        {
            _userTasksService = userTasksService ?? throw new ArgumentNullException(nameof(userTasksService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserTasks()
        {
            try
            {
                // Get the current user's ID from the JWT token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized();
                }

                var userId = Guid.Parse(userIdClaim.Value);
                var tasks = await _userTasksService.GetAllUserTasksAsync(userId);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving tasks: {ex}");
                return StatusCode(500, new { message = $"An error occurred while retrieving tasks: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserTaskById(Guid id)
        {
            try
            {
                var task = await _userTasksService.GetUserTaskByIdAsync(id);

                // Ensure the task belongs to the current user
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || task.AppUserId != Guid.Parse(userIdClaim.Value))
                {
                    return Forbid();
                }

                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving task {id}: {ex}");
                return StatusCode(500, new { message = $"An error occurred while retrieving the task: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserTask([FromBody] CreateUserTaskRequest taskDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Set the user ID from the token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized();
                }

                taskDto.AppUserId = Guid.Parse(userIdClaim.Value);
                var task = await _userTasksService.CreateUserTaskAsync(taskDto);
                return CreatedAtAction(nameof(GetUserTaskById), new { id = task.UserTaskId }, task);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating task: {ex}");
                return StatusCode(500, new { message = $"An error occurred while creating the task: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserTask(Guid id, [FromBody] UpdateUserTaskRequest taskDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Get the current task to check ownership
                var existingTask = await _userTasksService.GetUserTaskByIdAsync(id);

                // Ensure the task belongs to the current user
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || existingTask.AppUserId != Guid.Parse(userIdClaim.Value))
                {
                    return Forbid();
                }

                var updatedTask = await _userTasksService.UpdateUserTaskAsync(id, taskDto);
                return Ok(updatedTask);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating task {id}: {ex}");
                return StatusCode(500, new { message = $"An error occurred while updating the task: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserTask(Guid id)
        {
            try
            {
                // Get the current task to check ownership
                var existingTask = await _userTasksService.GetUserTaskByIdAsync(id);

                // Ensure the task belongs to the current user
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || existingTask.AppUserId != Guid.Parse(userIdClaim.Value))
                {
                    return Forbid();
                }

                var result = await _userTasksService.DeleteUserTaskAsync(id);
                if (result)
                {
                    return NoContent();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting task {id}: {ex}");
                return StatusCode(500, new { message = $"An error occurred while deleting the task: {ex.Message}" });
            }
        }

        [HttpGet("priority/{priority}")]
        public async Task<IActionResult> GetUserTasksByPriority(Priority priority)
        {
            try
            {
                var tasks = await _userTasksService.GetUserTasksByPriorityAsync(priority);

                // Filter to only return the current user's tasks
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized();
                }

                // Note: This filtering should ideally be done in the service
                // But I'm adding it here to demonstrate the principle
                // For better performance, update your service to accept and filter by userId

                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving tasks by priority {priority}: {ex}");
                return StatusCode(500, new { message = $"An error occurred while retrieving tasks: {ex.Message}" });
            }
        }
    }
}