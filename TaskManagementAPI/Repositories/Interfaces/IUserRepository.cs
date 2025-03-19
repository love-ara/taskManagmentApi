using TaskManagementAPI.Models.Entities;

namespace TaskManagementAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser> GetUserByIdAsync(Guid id);
        Task<AppUser> GetUserByEmailAsync(string email);
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<AppUser> GetUserByEmailOrUsernameAsync(string emailOrUsername);
        Task<AppUser> CreateUserAsync(AppUser user);
        Task<AppUser> UpdateUserAsync(AppUser user);
        Task<bool> DeleteUserAsync(Guid id);
        Task<bool> UserExistsByEmailAsync(string email);
        Task<bool> UserExistsByUsernameAsync(string username);
    }
}
