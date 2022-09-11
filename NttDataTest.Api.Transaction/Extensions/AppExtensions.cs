using Microsoft.AspNetCore.Builder;
using NttDataTest.Api.Transaction.Middlewares;

namespace NttDataTest.Api.Transaction.Extensions
{
    public static class AppExtensions
    {
        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
