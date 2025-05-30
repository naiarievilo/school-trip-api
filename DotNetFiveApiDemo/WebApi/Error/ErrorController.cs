using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace DotNetFiveApiDemo.WebApi.Error
{
    [ApiController]
    [SwaggerTag("[Internal] Manages exceptions thrown by the API.")]
    public class ErrorController : ControllerBase
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/error-local-development")]
        [SwaggerOperation("Returns detailed error when developing locally")]
        public IActionResult ErrorLocalDevelopment([FromServices] IWebHostEnvironment env)
        {
            if (!env.EnvironmentName.Equals("Development"))
                return NotFound();

            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            return Problem(
                context?.Error.StackTrace,
                title: context?.Error.Message,
                statusCode: 500,
                instance: context?.Error.Source,
                type: context?.Error.GetType().FullName
            );
        }

        [HttpGet("/error")]
        [SwaggerOperation("Returns default error")]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            _logger.LogError(context?.Error, context?.Error.Message);
            return Problem();
        }
    }
}