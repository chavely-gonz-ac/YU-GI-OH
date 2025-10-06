using MediatR;
using Microsoft.AspNetCore.Mvc;
using YuGiOh.Application.Features.Auth.Commands;

namespace YuGiOh.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : APIControllerBase
    {
        public AuthenticationController(IMediator mediator) : base(mediator) { }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticateCommand request)
        {
            if (request == null) return BadRequest("Invalid request data.");
            if (string.IsNullOrWhiteSpace(request.Handler) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Handler and password are required.");

            // Auto-fill IP if missing
            if (string.IsNullOrWhiteSpace(request.IpAddress))
            {
                request.IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            }

            try
            {
                var tokens = await Sender.Send(request);

                // Store refresh token securely in HttpOnly cookie
                Response.Cookies.Append("refreshToken", tokens.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // requires HTTPS
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(7) // match your refresh token expiry
                });

                return Ok(new
                {
                    AccessToken = tokens.AccessToken
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            // Pull refresh token from cookie, not request body
            var refreshToken = Request.Cookies["refreshToken"];
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            if (string.IsNullOrWhiteSpace(refreshToken))
                return BadRequest("Refresh token cookie missing.");

            try
            {
                var tokens = await Sender.Send(new RefreshTokenCommand
                {
                    RefreshToken = refreshToken,
                    IpAddress = ipAddress
                });

                // Rotate refresh token cookie
                Response.Cookies.Append("refreshToken", tokens.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(7)
                });

                return Ok(new
                {
                    AccessToken = tokens.AccessToken
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }
    }
}
