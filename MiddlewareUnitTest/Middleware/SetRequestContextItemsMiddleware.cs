using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MiddlewareUnitTest.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class SetRequestContextItemsMiddleware
    {
        private readonly RequestDelegate _next;
        public const string XRequestIdKey = "X-Request-ID";

        public SetRequestContextItemsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            // Read the Incoming Request Headers for a xRequestId Header
            // If present in the Header pull it from the Headers
            // Else Create a new Guid that represents the xRequestId


            //Set the xRequestId value thus obtained into HttpContext.Items Dictionary
            var headers = httpContext.Request.Headers
                    .ToDictionary(x => x.Key, x => x.Value.ToString());

            string xRequestIdValue = string.Empty;
            if (headers.ContainsKey(XRequestIdKey))
            {
                xRequestIdValue = headers[XRequestIdKey];
            }
            else
            {
                xRequestIdValue = Guid.NewGuid().ToString();
            }

            httpContext.Items.Add(XRequestIdKey, xRequestIdValue);

            await _next.Invoke(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SetRequestContextItemsMiddleware>();
        }
    }
}
