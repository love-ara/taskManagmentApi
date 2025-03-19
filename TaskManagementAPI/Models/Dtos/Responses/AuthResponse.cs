namespace TaskManagementAPI.Models.Dtos.Responses
{
    public class AuthResponse
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
       
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }

        public string Role { get; set; }

    }
}
