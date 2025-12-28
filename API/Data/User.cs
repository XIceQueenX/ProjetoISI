namespace Trabalho_ISI.Data
{
    /// <summary>
    /// Represents an application user.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Unique user identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User's username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// User's email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Hashed password.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// User role (default: User).
        /// </summary>
        public string Role { get; set; } = "User";

        /// <summary>
        /// Account creation date (UTC).
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// DTO used for user login.
    /// </summary>
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    /// <summary>
    /// DTO used for user registration.
    /// </summary>
    public class RegisterDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// User role (default: User).
        /// </summary>
        public string Role { get; set; } = "User";
    }

    /// <summary>
    /// DTO returned after successful authentication.
    /// </summary>
    public class AuthResponseDto
    {
        /// <summary>
        /// JWT authentication token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Authenticated user's username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Token expiration date.
        /// </summary>
        public DateTime Expiration { get; set; }
    }
}
