using System;
using System.Text;
using TaskManagementAPI.Models.Dtos.Requests;
using TaskManagementAPI.Models.Dtos.Responses;
using TaskManagementAPI.Models.Entities;
using TaskManagementAPI.Repositories.Interfaces;
using TaskManagementAPI.Services.Interfaces;

namespace TaskManagementAPI.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;


        public AuthService(IUserRepository userRepository, JwtService jwtService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest registerRequest)
        {
            // Check if user already exists
            if (await _userRepository.UserExistsByEmailAsync(registerRequest.Email))
            {
                throw new ApplicationException("User with this email already exists");
            }

            if (await _userRepository.UserExistsByUsernameAsync(registerRequest.Username))
            {
                throw new ApplicationException("User with this username already exists");
            }
           
          
            // Create password hash
            if (string.IsNullOrEmpty(registerRequest.Password))
            {
                throw new ArgumentException("Password cannot be empty");
            }

            // Create user
            var user = new AppUser
            {
                Username = registerRequest.Username,
                Email = registerRequest.Email,
                Role = "User"
            };

            // Hash password
           
            user.PasswordHash = HashPassword(registerRequest.Password); ;

            // Save user
            try
            {
                var createdUser = await _userRepository.CreateUserAsync(user);

                // Generate token
                var token = _jwtService.GenerateJwtToken(createdUser.Id, createdUser.Username, createdUser.Email);

                return new AuthResponse
                {
                    Token = token,
                    UserId = createdUser.Id,
                    Username = createdUser.Username,
                    Email = createdUser.Email,
                    Role = createdUser.Role
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error creating user: {ex.Message}");
            }
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest loginRequest)
        {
            // Check if user exists
            var user = await _userRepository.GetUserByEmailOrUsernameAsync(loginRequest.Username);
            if (user == null)
            {
                throw new ApplicationException("Invalid credentials");
            }

            // Verify password
            if (!VerifyPassword(loginRequest.Password, user.PasswordHash))
            {
                throw new ApplicationException("Invalid credentials");
            }

            // Generate token
            var token = _jwtService.GenerateJwtToken(user.Id, user.Username, user.Email);

            return new AuthResponse
            {
                Token = token,
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
             
            };
        }

       
        private String HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            string hashedPassword = HashPassword(password);
            return hashedPassword == storedHash;
        }
    }
}