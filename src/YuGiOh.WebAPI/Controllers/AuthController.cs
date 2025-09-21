using MediatR;
using Microsoft.AspNetCore.Mvc;

using YuGiOh.Application.Features.Auth.Queries;
using YuGiOh.Application.Features.Auth.Commands;
using YuGiOh.Domain.Models.DTOs;

namespace YuGiOh.WebAPI.Controllers
{
    public class AuthController : APIControllerBase
    {

        public AuthController(IMediator mediator) : base(mediator) { }

        /// <summary>
        /// Registers a new user (Player or Sponsor).
        /// </summary>
        /// <param name="request">User registration data.</param>
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            if (request == null) return BadRequest("Invalid request");
            Console.WriteLine("I am here.");

            try
            {
                // Dispatch registration command
                var token = await Sender.Send(new RegisterCommand { Data = request });
            Console.WriteLine("I am here.");

                // Build confirmation callback URL
                var callbackUrl = Url.Action(
                    nameof(ConfirmEmail),
                    "Auth",
                    new { email = request.Email, token },
                    Request.Scheme);
            Console.WriteLine("I am here.");

                // Dispatch confirmation email command
                await Sender.Send(new SendConfirmationEmailCommand
                {
                    Email = request.Email,
                    CallbackURL = callbackUrl!
                });
            Console.WriteLine("I am here.");

                return Ok(new { Message = "Registration successful. Please confirm your email." });
            }
            catch (Exception ex)
            {
            Console.WriteLine(ex.Message);
                return StatusCode(500, "An error occurred during registration.");
            }
        }

        /// <summary>
        /// Confirms a user's email using the token.
        /// </summary>
        /// <param name="email">The registered email address.</param>
        /// <param name="token">The confirmation token.</param>
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
            {
                return Content(BuildHtmlPage("Invalid email or token.", "http://localhost:3000/login"), "text/html");
            }

            try
            {
                var command = new ConfirmEmailQuery { Email = email, Token = token };
                var result = await Sender.Send(command);

                if (result)
                    return Content(BuildHtmlPage("Email confirmed successfully!", "http://localhost:3000/login"), "text/html");

                return Content(BuildHtmlPage("Invalid or expired confirmation link.", "http://localhost:3000/login"), "text/html");
            }
            catch (Exception)
            {
                return Content(BuildHtmlPage("An error occurred while confirming your email.", "http://localhost:3000/login"), "text/html");
            }
        }

        /// <summary>
        /// Builds a standalone HTML page with a message and a link to the frontend login page (violet theme).
        /// </summary>
        private string BuildHtmlPage(string message, string loginUrl)
        {
            return $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Email Confirmation</title>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background: linear-gradient(135deg, #e0bbff, #d1a3ff);
                        display: flex;
                        justify-content: center;
                        align-items: center;
                        height: 100vh;
                        margin: 0;
                    }}
                    .card {{
                        background-color: #fff;
                        padding: 2rem 3rem;
                        border-radius: 12px;
                        text-align: center;
                        box-shadow: 0 8px 16px rgba(0,0,0,0.2);
                    }}
                    h1 {{
                        color: #4b0082;
                        margin-bottom: 1rem;
                    }}
                    a {{
                        display: inline-block;
                        margin-top: 1rem;
                        padding: 0.75rem 1.5rem;
                        background-color: #8a2be2;
                        color: #fff;
                        text-decoration: none;
                        border-radius: 6px;
                        font-weight: bold;
                        transition: background-color 0.3s;
                    }}
                    a:hover {{
                        background-color: #7a1ecf;
                    }}
                </style>
            </head>
            <body>
                <div class='card'>
                    <h1>{message}</h1>
                    <a href='{loginUrl}'>Go to Login</a>
                </div>
            </body>
            </html>";
        }


    }
}
