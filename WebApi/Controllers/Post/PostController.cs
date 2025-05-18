using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebApi.Controllers.Post
{
    [Route("api/[controller]")]
    [ApiController] 
    public class PostController : BaseApiController
    {
        [HttpPost("CreatePost")]
        [Authorize]
        public async Task<IActionResult> CreatePost([FromBody] Application.DTOs.PostDto postDto)
        {
            try 
            {
                if (!postDto.Attributes.Any(p =>
                    !string.IsNullOrWhiteSpace(p.Name) && p.Name.ToLower() != "string")) postDto.Attributes = null;
                    // Validate the request
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await Mediator.Send(new Application.Features.PostFeature.Command.CreatePostCommand
                {
                    PostDto = postDto,
                    CancellationToken = HttpContext.RequestAborted
                });
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error creating post ");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
          
        }

        [HttpGet("GetListPost")]
        public async Task<IActionResult> GetListPost([FromQuery] Application.Features.PostFeature.Queries.GetListPostQueries request)
        {
            try
            {
                var result = await Mediator.Send(new Application.Features.PostFeature.Queries.GetListPostQueries
                {
                    page = request.page,
                    pageSize = request.pageSize,
                    CancellationToken = HttpContext.RequestAborted
                });
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error getting list post");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
