using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        private IMediator _mediator;
        private ILogger<BaseApiController> _logger;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        protected ILogger<BaseApiController> Logger => _logger ??= HttpContext.RequestServices.GetService<ILogger<BaseApiController>>();
    }
}