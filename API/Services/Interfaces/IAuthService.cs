using Trabalho_ISI.Data;

namespace Trabalho_ISI.Services.Interfaces
{
    /// <summary>
    /// Service interface for user authentication.
    /// Handles registration and login operations.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Registers a new user with the provided registration data.
        /// Returns an authentication response with a token.
        /// </summary>
        /// <param name="dto">User registration information</param>
        /// <returns>Authentication response including JWT token</returns>
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);

        /// <summary>
        /// Logs in a user with the provided credentials.
        /// Returns an authentication response with a token if successful.
        /// </summary>
        /// <param name="dto">User login credentials</param>
        /// <returns>Authentication response including JWT token</returns>
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
    }
}
