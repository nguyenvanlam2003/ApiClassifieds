using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers.Category
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : BaseApiController
    {
        [HttpPost("CreateCategory")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory([FromBody] Application.DTOs.CategoryDto categoryDto, CancellationToken cancellation)
        {
            try
            {
                if (categoryDto.ParentId == 0) categoryDto.ParentId = null;
                if (!categoryDto.Children.Any(c =>
                    !string.IsNullOrWhiteSpace(c.Name) && c.Name.ToLower() != "string")) categoryDto.Children = null;
                // Validate the request
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                // Call the command handler to handle the sign-up logic
                var result = await Mediator.Send(new Application.Features.CategoryFeature.Command.CreateCategoryCommand { Category = categoryDto, CancellationToken = cancellation });
                // Return the result
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error creating category ");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("GetListCategory")]
        public async Task<IActionResult> GetListCategory([FromQuery] int page = 1, [FromQuery] int pageSize = 15, CancellationToken cancellation = default)
        {
            try
            {
                // Validate the request
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                // Call the command handler to handle the sign-up logic
                var result = await Mediator.Send(new Application.Features.CategoryFeature.Queries.GetListCategoryQueries { page = page, pageSize = pageSize, CancellationToken = cancellation });
                // Return the result
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error getting list category ");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPut("UpdateCategory")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory([FromBody] Application.DTOs.CategoryDto categoryDto, CancellationToken cancellation)
        {
            try
            {
                // Validate the request
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                // Call the command handler to handle the sign-up logic
                var result = await Mediator.Send(new Application.Features.CategoryFeature.Command.UpdateCategoryCommand { Category = categoryDto, CancellationToken = cancellation });
                // Return the result
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating category ");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("GetCategoryById")]
        [SwaggerOperation(Summary = "Lấy danh sách danh mục theo ParentId", Description = "Truyền vào một ParentId để lấy danh sách các danh mục con.")]
        public async Task<IActionResult> GetCategoryById([FromQuery, SwaggerParameter("Id truyền vào là id của danh mục cha (ParentId)") ] int ParentId, CancellationToken cancellation)
        {
            try
            {
                // Validate the request
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                // Call the command handler to handle the sign-up logic
                var result = await Mediator.Send(new Application.Features.CategoryFeature.Queries.GetCategoryByIdQueries{ Id = ParentId, CancellationToken = cancellation });
                // Return the result
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error getting category by ID ");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpDelete("DeleteCategoryById")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategoryById([FromQuery] int id, CancellationToken cancellation)
        {
            try
            {
                // Validate the request
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                // Call the command handler to handle the sign-up logic
                var result = await Mediator.Send(new Application.Features.CategoryFeature.Command.DeleteCategoryCommand { Id = id, CancellationToken = cancellation });
                // Return the result
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error deleting category by ID ");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
