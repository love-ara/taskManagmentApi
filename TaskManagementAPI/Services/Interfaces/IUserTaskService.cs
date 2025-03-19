using TaskManagementAPI.Models.Dtos.Requests;
using TaskManagementAPI.Models.Entities;

namespace TaskManagementAPI.Services.Interfaces
{
    public interface IUserTasksService
    {
        Task<IEnumerable<UserTaskDto>> GetAllUserTasksAsync(Guid userId);
        Task<UserTaskDto> GetUserTaskByIdAsync(Guid id);
        Task<UserTaskDto> CreateUserTaskAsync(CreateUserTaskRequest request);
        Task<UserTaskDto> UpdateUserTaskAsync(Guid id, UpdateUserTaskRequest request);
        Task<bool> DeleteUserTaskAsync(Guid id);
        Task<IEnumerable<UserTaskDto>> GetUserTasksByPriorityAsync(Priority priority);
    }
}
