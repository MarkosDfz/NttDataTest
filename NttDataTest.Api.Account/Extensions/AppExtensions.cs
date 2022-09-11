using Microsoft.AspNetCore.Builder;
using NttDataTest.Api.Account.Middlewares;

namespace NttDataTest.Api.Account.Extensions
{
    public static class AppExtensions
    {
        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
