namespace Trabalho_ISI.Data
{

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } = "User"; // default role
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }


    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class RegisterDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "User"; // default role
    }


    public class AuthResponseDto
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public DateTime Expiration { get; set; }
    }
}
