using Microsoft.AspNetCore.Builder;
using NttDataTest.Api.Client.Middlewares;

namespace NttDataTest.Api.Client.Extensions
{
    public static class AppExtensions
    {
        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
