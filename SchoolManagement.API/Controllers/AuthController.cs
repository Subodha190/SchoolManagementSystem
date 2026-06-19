using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Application.Common.Models.Auth;
using SchoolManagement.Application.Services;
using SchoolManagement.API.Models;
using Microsoft.AspNetCore.Authorization;

namespace SchoolManagement.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(
            AuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto request)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var tokens = await _authService.RegisterAsync(request, ip);

            Response.Cookies.Append("refreshToken", tokens.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(tokens);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var tokens = await _authService.LoginAsync(request, ip);

            Response.Cookies.Append("refreshToken", tokens.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(tokens);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var token = request?.RefreshToken ?? Request.Cookies["refreshToken"];
            var tokens = await _authService.RefreshAsync(token, ip);

            Response.Cookies.Append("refreshToken", tokens.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(tokens);
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke([FromBody] RevokeRequest request)
        {
            var token = request?.RefreshToken ?? Request.Cookies["refreshToken"];
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            await _authService.RevokeAsync(token, ip);
            Response.Cookies.Delete("refreshToken");
            return NoContent();
        }
    }
}
