using Microsoft.AspNetCore.Http;

namespace Dukkani.Shared.Middlewares
{
    public class ListenToApiGateway(RequestDelegate next)
    {
        public async Task InvoiceAsync(HttpContext context)
        {
            //only for requests which header contains some key for api gateway
            var signedHeader = context.Request.Headers["Api-Gateway"];
            //Null means the request came not from api gateway
            if (signedHeader.FirstOrDefault() is null)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("Sorry, Service is unavailable");
                return;
            }
            else
            {
                await next(context);
            }
        }
    }
}
