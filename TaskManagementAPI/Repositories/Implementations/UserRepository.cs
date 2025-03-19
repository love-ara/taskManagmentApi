using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models.Entities;
using TaskManagementAPI.Repositories.Interfaces;

namespace TaskManagementAPI.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<AppUser> GetUserByIdAsync(Guid id)
        {
            var user = await _context.AppUsers.FindAsync(id);
            if (user == null)
            {
                throw new Exception($"User with id {id} not found");
            }
            return user;
        }

        public async Task<AppUser> GetUserByEmailAsync(string email)
        {
            return await _context.AppUsers.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.AppUsers.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<AppUser> GetUserByEmailOrUsernameAsync(string emailOrUsername)
        {
            return await _context.AppUsers.FirstOrDefaultAsync(u =>
                u.Email == emailOrUsername || u.Username == emailOrUsername);
        }

        public async Task<AppUser> CreateUserAsync(AppUser user)
        {
            user.Id = Guid.NewGuid();
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.AppUsers.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<AppUser> UpdateUserAsync(AppUser user)
        {
            var existingUser = await _context.AppUsers.FindAsync(user.Id);
            if (existingUser == null)
            {
                throw new Exception($"User with id {user.Id} not found");
            }

            user.UpdatedAt = DateTime.UtcNow;
            _context.Entry(existingUser).CurrentValues.SetValues(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _context.AppUsers.FindAsync(id);
            if (user == null)
            {
                throw new Exception($"User with id {id} not found");
            }

            _context.AppUsers.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UserExistsByEmailAsync(string email)
        {
            return await _context.AppUsers.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> UserExistsByUsernameAsync(string username)
        {
            return await _context.AppUsers.AnyAsync(u => u.Username == username);
        }
    }

}
