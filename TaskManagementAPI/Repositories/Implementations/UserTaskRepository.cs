using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models.Entities;
using TaskManagementAPI.Repositories.Interfaces;

namespace TaskManagementAPI.Repositories.Implementations
{
    public class UserTaskRepository : IUserTaskRepository
    {
        private readonly ApplicationDbContext _context;

        public UserTaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserTask>> GetAllUserTasksAsync()
        {
            return await _context.UserTasks
                .Include(t => t.Tags)
                .ToListAsync();
        }

        public async Task<UserTask> GetUserTaskByIdAsync(Guid id)
        {
            return await _context.UserTasks
                .Include(t => t.Tags)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<UserTask> CreateUserTaskAsync(UserTask task)
        {
            // Handle tags
            var tagsList = new List<Tag>();

            foreach (var tag in task.Tags)
            {
                var existingTag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tag.Name);
                if (existingTag != null)
                {
                    tagsList.Add(existingTag);
                }
                else
                {
                    tagsList.Add(tag);
                }
            }

            task.Tags = tagsList;

            await _context.UserTasks.AddAsync(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<UserTask> UpdateUserTaskAsync(UserTask task)
        {
            // Handle tags
            var tagsList = new List<Tag>();

            foreach (var tag in task.Tags)
            {
                var existingTag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tag.Name);
                if (existingTag != null)
                {
                    tagsList.Add(existingTag);
                }
                else
                {
                    tagsList.Add(tag);
                }
            }

            task.Tags = tagsList;
            task.UpdatedAt = DateTime.UtcNow;

            _context.UserTasks.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> DeleteUserTaskAsync(Guid id)
        {
            var task = await _context.UserTasks.FindAsync(id);
            if (task == null)
                return false;

            _context.UserTasks.Remove(task);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<UserTask>> GetUserTasksByPriorityAsync(Priority priority)
        {
            return await _context.UserTasks
                .Include(t => t.Tags)
                .Where(t => t.Priority == priority)
                .ToListAsync();
        }

        public async Task<bool> UserTaskExistsAsync(Guid id)
        {
            return await _context.UserTasks.AnyAsync(t => t.Id == id);
        }
    }
}