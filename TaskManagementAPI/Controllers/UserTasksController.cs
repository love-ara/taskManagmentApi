using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskManagementAPI.Models.Dtos.Requests;
using TaskManagementAPI.Models.Dtos.Responses;
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
        public async Task<ActionResult<ApiResponse<IEnumerable<UserTaskDto>>>> GetUserAllTasks()
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
                return Ok(ApiResponse<IEnumerable<UserTaskDto>>.SuccessResponse(tasks, "Task retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving tasks: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  ApiResponse<IEnumerable<UserTaskDto>>.FailureResponse("An error occurred while retrieving tasks. Please try again later."));
            }
        }

       [HttpGet("byId")]
        public async Task<ActionResult<ApiResponse<UserTaskDto>>> GetUserTaskById([FromQuery] Guid id)
        {
            try
            {
                var task = await _userTasksService.GetUserTaskByIdAsync(id);

                // Ensure the task belongs to the current user
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || task.AppUserId != Guid.Parse(userIdClaim.Value))
                {
                    return NotFound(ApiResponse<UserTaskDto>.FailureResponse($"Task with ID {id} not found"));
                }

                return Ok(ApiResponse<UserTaskDto>.SuccessResponse(task, "Task retrieved successfully"));

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving task {id}: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<UserTaskDto>.FailureResponse("An error occurred while retrieving the task. Please try again later."));            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<UserTaskDto>>> CreateUserTask([FromBody] CreateUserTaskRequest taskRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<UserTaskDto>.FailureResponse("Invalid task data"));
                }

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized(ApiResponse<UserTaskDto>.FailureResponse("User not authenticated"));
                }

                taskRequest.AppUserId = Guid.Parse(userIdClaim.Value);
                var task = await _userTasksService.CreateUserTaskAsync(taskRequest);
                return CreatedAtAction(nameof(GetUserTaskById), new { id = task.UserTaskId }, ApiResponse<UserTaskDto>.SuccessResponse(task, "Task created successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating task: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<UserTaskDto>.FailureResponse("An error occurred while creating the task. Please try again later."));
            }
        }


            [HttpPatch]
        public async Task<ActionResult<ApiResponse<UserTaskDto>>> UpdateUserTask(Guid id, [FromBody] UpdateUserTaskRequest updateTaskRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<UserTaskDto>.FailureResponse("Invalid task data"));
                }

                var existingTask = await _userTasksService.GetUserTaskByIdAsync(id);

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || existingTask.AppUserId != Guid.Parse(userIdClaim.Value))
                {
                     return NotFound(ApiResponse<UserTaskDto>.FailureResponse($"Task with ID {id} not found"));
                }

                var updatedTask = await _userTasksService.UpdateUserTaskAsync(id, updateTaskRequest);
                return Ok(ApiResponse<UserTaskDto>.SuccessResponse(updatedTask, "Task updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating task {id}: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<UserTaskDto>.FailureResponse("An error occurred while updating the task. Please try again later."));
            }
        }

        [HttpDelete]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteUserTask([FromQuery] Guid id)
        {
            try
            {
                // Get the current task to check ownership
                var existingTask = await _userTasksService.GetUserTaskByIdAsync(id);

                // Ensure the task belongs to the current user
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || existingTask.AppUserId != Guid.Parse(userIdClaim.Value))
                {
                    return NotFound(ApiResponse<bool>.FailureResponse("User not found"));

                }

                var result = await _userTasksService.DeleteUserTaskAsync(id);
                if (result)
                {
                    return NoContent();
                }
                return NotFound(ApiResponse<bool>.FailureResponse($"Task with ID {id} not found"));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting task {id}: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<bool>.FailureResponse("An error occurred while deleting the task. Please try again later."));
            }
        }

        [HttpGet("priority/{priority}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserTaskDto>>>> GetUserTasksByPriority(Priority priority)
        {
            try
            {
                var tasks = await _userTasksService.GetUserTasksByPriorityAsync(priority);

                // Filter to only return the current user's tasks
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized(ApiResponse<bool>.FailureResponse("Invalid or expired token, Login again"));
                }      

                return Ok(ApiResponse<IEnumerable<UserTaskDto>>.SuccessResponse(tasks, $"Tasks with priority {priority} retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving tasks by priority {priority}: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<IEnumerable<UserTaskDto>>.FailureResponse("An error occurred while retrieving tasks. Please try again later."));
            }
        }
    }
}