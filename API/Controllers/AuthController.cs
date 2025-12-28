using Microsoft.AspNetCore.Mvc;
using Trabalho_ISI.Data;
using Trabalho_ISI.Services.Interfaces;

namespace Trabalho_ISI.Controllers
{
    /// <summary>
    /// Controller responsible for authentication actions such as
    /// user registration and login.
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        /// <summary>
        /// Constructor with dependency injection of the authentication service.
        /// </summary>
        /// <param name="authService">Authentication service</param>
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="dto">User registration data</param>
        /// <returns>Registered user or success message</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            try
            {
                var result = await _authService.RegisterAsync(dto);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Authenticates a user and returns a token or user data.
        /// </summary>
        /// <param name="dto">Login credentials</param>
        /// <returns>Authentication result or unauthorized response</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (result == null)
                return Unauthorized(new { message = "Invalid credentials" });

            return Ok(result);
        }
    }
}
