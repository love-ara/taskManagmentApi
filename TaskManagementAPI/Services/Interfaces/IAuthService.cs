using TaskManagementAPI.Models.Dtos.Requests;
using TaskManagementAPI.Models.Dtos.Responses;

namespace TaskManagementAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest registerRequest);
        Task<AuthResponse> LoginAsync(LoginRequest loginRequest);

    }
}
