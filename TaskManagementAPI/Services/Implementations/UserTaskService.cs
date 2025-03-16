using Microsoft.Extensions.Logging;
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

        public async Task<IEnumerable<UserTaskDto>> GetAllUserTasksAsync()
        {
            var tasks = await _taskRepository.GetAllUserTasksAsync();
            return tasks.Select(MapToDto);
        }

        public async Task<UserTaskDto> GetUserTaskByIdAsync(Guid id)
        {
            var task = await _taskRepository.GetUserTaskByIdAsync(id);
            return task != null ? MapToDto(task) : null;
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
                Tags = taskDto.Tags.Select(t => new Tag { Name = t }).ToList()
            };

            var createdTask = await _taskRepository.CreateUserTaskAsync(task);
            return MapToDto(createdTask);
        }

        public async Task<UserTaskDto> UpdateUserTaskAsync(Guid id, UpdateUserTaskRequest taskDto)
        {
            var existingTask = await _taskRepository.GetUserTaskByIdAsync(id);
            if (existingTask == null)
                return null;

            existingTask.Name = taskDto.Name;
            existingTask.Description = taskDto.Description;
            existingTask.IsComplete = taskDto.IsComplete;
            existingTask.StartDate = taskDto.StartDate;
            existingTask.DueDate = taskDto.DueDate;
            existingTask.Priority = taskDto.Priority;
            existingTask.Tags = taskDto.Tags.Select(t => new Tag { Name = t }).ToList();

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
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                IsComplete = task.IsComplete,
                StartDate = task.StartDate,
                DueDate = task.DueDate,
                Priority = task.Priority,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt,
                Tags = task.Tags.Select(t => t.Name).ToList()
            };
        }
    }
}
