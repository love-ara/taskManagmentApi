using TaskManagementAPI.Models.Entities;

namespace TaskManagementAPI.Repositories.Interfaces
{
    public interface IUserTaskRepository
    {
        Task<IEnumerable<UserTask>> GetAllUserTasksAsync();
        Task<UserTask> GetUserTaskByIdAsync(Guid id);
        Task<UserTask> CreateUserTaskAsync(UserTask task);
        Task<UserTask> UpdateUserTaskAsync(UserTask task);
        Task<bool> DeleteUserTaskAsync(Guid id);
        Task<IEnumerable<UserTask>> GetUserTasksByPriorityAsync(Priority priority);
        Task<bool> UserTaskExistsAsync(Guid id);
    }
}
