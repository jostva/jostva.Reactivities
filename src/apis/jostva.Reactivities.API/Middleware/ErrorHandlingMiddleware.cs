using jostva.Reactivities.application.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace jostva.Reactivities.API.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        private readonly ILogger<ErrorHandlingMiddleware> logger;


        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }


        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception, logger);
            }
        }


        private async Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger<ErrorHandlingMiddleware> logger)
        {
            object errors = null;

            switch (exception)
            {
                case RestException re:
                    logger.LogError(exception, "REST ERROR");
                    errors = re.Errors;
                    context.Response.StatusCode = (int)re.Code;
                    break;

                case Exception e:
                    logger.LogError(exception, "SERVER ERROR");
                    errors = string.IsNullOrWhiteSpace(e.Message) ? "ERROR" : e.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.ContentType = "application/json";
            if (errors != null)
            {
                //  TODO: Usar System.Text.Json en Core 3.0  JsonSerializer.Serialize(new { errors });
                var result = JsonConvert.SerializeObject(new
                {
                    errors
                });

                await context.Response.WriteAsync(result);
            }
        }
    }
}