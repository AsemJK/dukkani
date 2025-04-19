using Dukkani.Shared.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dukkani.Shared.Middlewares
{
    public class GlobalException(RequestDelegate next)
    {
        public async Task InvoiceAsync(HttpContext context)
        {
            //default response error variables
            var message = "";
            int statusCode = (int)StatusCodes.Status500InternalServerError;
            string title = "Error";

            try
            {
                await next(context);

                //if response is Too many requests / 429 status code
                if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    title = "Warning";
                    statusCode = (int)StatusCodes.Status429TooManyRequests;
                    message = "Too many request made.";
                    await ModifyHeader(context, title, message, statusCode);
                }
                //if the response is Unauthorized / 401 status code
                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    title = "Alert";
                    statusCode = (int)StatusCodes.Status401Unauthorized;
                    message = "You are not authorized to access";
                    await ModifyHeader(context, title, message, statusCode);
                }
                //if the response is Forbidden / 403 status code
                if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    title = "Out of Service";
                    statusCode = (int)StatusCodes.Status403Forbidden;
                    message = "You are not Allowed/Required to access";
                    await ModifyHeader(context, title, message, statusCode);
                }
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                if (ex is TaskCanceledException || ex is TimeoutException)
                {
                    title = "Out of Time";
                    message = "Request Timeout ... Try again";
                    statusCode = StatusCodes.Status408RequestTimeout;
                }

                await ModifyHeader(context, title, message, statusCode);
            }
        }

        private async Task ModifyHeader(HttpContext context, string title, string message, int statusCode)
        {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new ProblemDetails()
            {
                Title = title,
                Detail = message,
                Status = statusCode
            }));
        }
    }
}
