using Application.DTOs;
using Application.Features.AuthFeature.Command;
using Application.Features.AuthFeature.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseApiController
    {
        [HttpPost("Sign-up")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDto request, CancellationToken cancellation)
        {
            try
            {
                // Validate the request
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                // Call the command handler to handle the sign-up logic
                var result = await Mediator.Send(new SignUpCommand { Request = request, CancellationToken = cancellation });
                // Return the result
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error signup ");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
        [HttpPost("login-in")]
        public async Task<IActionResult> LoginIn([FromQuery] string Email, [FromQuery] string Password, CancellationToken cancellation)
        {
            try
            {
                // Validate the request
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                // Call the command handler to handle the sign-up logic
                var result = await Mediator.Send(new LogInQueries { Email = Email, Password = Password, CancellationToken = cancellation });
                // Return the result
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error login-in ");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
