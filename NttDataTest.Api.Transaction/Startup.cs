using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NttDataTest.Api.Transaction.Extensions;
using NttDataTest.Persistence.Transaction;
using NttDataTest.Persistence.Transaction.Repository;
using NttDataTest.Services.Proxies.Transaction;
using NttDataTest.Services.Proxies.Transaction.Account;
using NttDataTest.Services.Transaction.Behaviors;
using NttDataTest.Services.Transaction.Interfaces;
using System.Reflection;

namespace NttDataTest.Api.Transaction
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TransactionDataContext>(opts =>
                opts.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")
                )
            );

            services.AddTransient(typeof(IRepositoryAsync<>), typeof(MyRepositoryAsync<>));

            services.AddMediatR(Assembly.Load("NttDataTest.Services.Transaction"));

            services.AddAutoMapper(Assembly.Load("NttDataTest.Services.Transaction"));

            services.AddValidatorsFromAssembly(Assembly.Load("NttDataTest.Services.Transaction"));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.Configure<ApiUrls>(opts => Configuration.GetSection("ApiUrls").Bind(opts));

            services.AddHttpClient<IAccountProxy, AccountProxy>();
            services.AddHttpClient<IAccountUpdateInitialBalanceProxy, AccountUpdateInitialBalanceProxy>();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NttDataTest.Api.Transaction", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NttDataTest.Api.Transaction v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseErrorHandlingMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
