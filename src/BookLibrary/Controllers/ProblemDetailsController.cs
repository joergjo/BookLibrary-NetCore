using System;
using System.Diagnostics;
using BookLibrary.Common;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BookLibrary.Controllers
{
    [Route("error")]
    [ApiController]
    public class ProblemDetailsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _environment;

        public ProblemDetailsController(
            ILogger<ProblemDetailsController> logger,
            IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        [Produces("application/problem+json", "application/problem+xml")]
        public ActionResult<ProblemDetails> Error()
        {
            string requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            var problemDetails = new ProblemDetails
            {
                Title = "Internal Server Error",
                Status = 500,
                Type = "about:blank",
                Instance = "about:blank",
                Detail = "An unhandled exception occurred. Use the value of the 'request-id' extension to track the error.",
            };
            problemDetails.Extensions.Add("request-id", requestId);
            problemDetails.Extensions.Add("timestamp-utc", DateTimeOffset.UtcNow);

            var errorFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (errorFeature?.Error is object)
            {
                _logger?.LogError(
                    ApplicationEvents.UnhandledException,
                    errorFeature.Error,
                    "Caught unhandled exception for request ID '{RequestId}'.",
                    requestId);

                if (_environment?.IsDevelopment() == true)
                {
                    problemDetails.Extensions.Add(
                        "debug-exception-type",
                        errorFeature.Error.GetType().FullName);
                    problemDetails.Extensions.Add(
                        "debug-exception-message",
                        errorFeature.Error.Message);
                    problemDetails.Extensions.Add(
                        "debug-exception-stacktrace",
                        errorFeature.Error.StackTrace);
                }
            }

            // Note that the appropriate status code (i.e. HTTP 500) is set through the
            // ExceptionHandlerMiddleware. We only return the problem description content.
            return problemDetails;
        }

        [Route("test")]
        public ActionResult Test()
        {
            if (_environment?.IsDevelopment() == true)
            {
                throw new ApplicationException("Ouch! We just caused an unhandled exception");
            }
            return NotFound();
        }
    }
}
