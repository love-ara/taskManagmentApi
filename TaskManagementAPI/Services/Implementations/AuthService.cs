using System;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
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
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
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
                var token = GenerateJwtToken(createdUser.Id, createdUser.Username, createdUser.Email);

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
            var token = GenerateJwtToken(user.Id, user.Username, user.Email);

            return new AuthResponse
            {
                Token = token,
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
             
            };
        }

        public string GenerateJwtToken(Guid userId, string username, string email)
        {
            var jwtKey = _configuration["JWT:SecretKey"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new ApplicationException("JWT key is not configured");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Email, email)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
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