using Application.DTOs;
using Application.Features.UserFeature.Command;
using Application.Features.UserFeature.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseApiController
    {

        [HttpGet("GetUserById/{id}")]
        [Authorize()]
        public async Task<IActionResult> GetUserById(int id, CancellationToken cancellation)
        {
            try
            {
                // Validate the request
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                // Call the command handler to handle the sign-up logic
                var result = await Mediator.Send(new GetUserByIdQuery { Id = id, CancellationToken = cancellation });
                // Return the result
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error getting user by ID ");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellation)
        {
            try
            {
                // Validate the request
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                // Call the command handler to handle the sign-up logic
                var result = await Mediator.Send(new GetAllUsersQuery { CancellationToken = cancellation });
                // Return the result
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error getting all users ");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("RequestSendOtp")]
        public async Task<IActionResult> RequestChangePassword([FromQuery] string email,
            [FromQuery, SwaggerParameter("Type of request. Possible values:  change_password , sign_up")] string? requestType ,  CancellationToken cancellation )
        {
            try
            {
                // Validate the request
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                // Call the command handler to handle the sign-up logic
                var result = await Mediator.Send(new RequestChangePasswordCommand { Email = email, RequestType = requestType, CancellationToken = cancellation });
                // Return the result
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error requesting change password ");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordResetDto passwordReset, CancellationToken cancellation)
        {
            try
            {
                // Validate the request
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                // Call the command handler to handle the sign-up logic
                var result = await Mediator.Send(new ChangePasswordCommand { passwordReset = passwordReset, cancellation = cancellation });
                // Return the result
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error changing password ");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPut("UpdateUser")]
        [Authorize()]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto user, CancellationToken cancellation)
        {
            try
            {
                // Validate the request
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                // Call the command handler to handle the sign-up logic
                var result = await Mediator.Send(new UpdateUserCommand { User = user, CancellationToken = cancellation });
                // Return the result
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating user ");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
