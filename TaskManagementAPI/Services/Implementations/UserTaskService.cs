using TaskManagementAPI.Models.Dtos.Requests;
using TaskManagementAPI.Models.Entities;
using TaskManagementAPI.Repositories.Interfaces;
using TaskManagementAPI.Services.Interfaces;

namespace TaskManagementAPI.Services.Implementations
{
    public class UserTaskService : IUserTasksService
    {
        private readonly IUserTaskRepository _taskRepository;
        private readonly ILogger<UserTaskService> _logger;

        public UserTaskService(IUserTaskRepository taskRepository, ILogger<UserTaskService>  logger)
        {
            _taskRepository = taskRepository;
            _logger = logger;

        }

        public async Task<IEnumerable<UserTaskDto>> GetAllUserTasksAsync(Guid userId)
        {
            var userTasks = await _taskRepository.GetAllUserTasksAsync(userId);
            if (userTasks == null)
            {
                throw new Exception("No Tasks found");
            }
                return userTasks.Select(task => MapToDto(task));
        }

        public async Task<UserTaskDto> GetUserTaskByIdAsync(Guid id)
        {
            var task = await _taskRepository.GetUserTaskByIdAsync(id);
            return task != null ? MapToDto(task) : throw new Exception($"Task with id {id} not found");
        }

        public async Task<UserTaskDto> CreateUserTaskAsync(CreateUserTaskRequest taskDto)
        {
            var task = new UserTask
            {
                Name = taskDto.Name,
                Description = taskDto.Description,
                IsComplete = taskDto.IsComplete,
                StartDate = taskDto.StartDate,
                DueDate = taskDto.DueDate,
                Priority = taskDto.Priority,
                Tags = taskDto.Tags.Select(t => new Tag { Name = t }).ToList(),
                AppUserId = taskDto.AppUserId
            };

            var createdTask = await _taskRepository.CreateUserTaskAsync(task);
            return MapToDto(createdTask);
        }

        public async Task<UserTaskDto> UpdateUserTaskAsync(Guid id, UpdateUserTaskRequest taskDto)
        {
            var existingTask = await _taskRepository.GetUserTaskByIdAsync(id);
            if (existingTask == null)
            {
                throw new Exception($"Task with id {id} not found.");
            }
        
            existingTask.Name = taskDto.Name;
            existingTask.Description = taskDto.Description;
            existingTask.IsComplete = taskDto.IsComplete;
            existingTask.StartDate = taskDto.StartDate;
            existingTask.DueDate = taskDto.DueDate;
            existingTask.Priority = taskDto.Priority;
            existingTask.Tags = taskDto.Tags.Select(t => new Tag { Name = t }).ToList();

            existingTask.UpdatedAt = DateTime.Now;
            var updatedTask = await _taskRepository.UpdateUserTaskAsync(existingTask);
            return MapToDto(updatedTask);
        }

        public async Task<bool> DeleteUserTaskAsync(Guid id)
        {
            return await _taskRepository.DeleteUserTaskAsync(id);
        }

        public async Task<IEnumerable<UserTaskDto>> GetUserTasksByPriorityAsync(Priority priority)
        {
            var tasks = await _taskRepository.GetUserTasksByPriorityAsync(priority);
            return tasks.Select(MapToDto);
        }

        private UserTaskDto MapToDto(UserTask task)
        {
            return new UserTaskDto
            {
                UserTaskId = task.Id,
                Name = task.Name,
                Description = task.Description,
                IsComplete = task.IsComplete,
                StartDate = task.StartDate,
                DueDate = task.DueDate,
                Priority = task.Priority,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt,
                Tags = task.Tags.Select(t => t.Name).ToList(),
                AppUserId = task.AppUserId
            };
        }
    }
}
