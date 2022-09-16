using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NttDataTest.Api.Account.Extensions;
using NttDataTest.Persistence.Account;
using NttDataTest.Persistence.Account.Repository;
using NttDataTest.Services.Account.Behaviors;
using NttDataTest.Services.Account.Interfaces;
using NttDataTest.Services.Proxies.Account;
using NttDataTest.Services.Proxies.Account.Client;
using NttDataTest.Services.Proxies.Account.Transaction;
using System.Globalization;
using System.Reflection;

namespace NttDataTest.Api.Account
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
            services.AddDbContext<AccountDataContext>(opts =>
                opts.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")
                )
            );

            services.AddTransient(typeof(IRepositoryAsync<>), typeof(MyRepositoryAsync<>));

            services.AddMediatR(Assembly.Load("NttDataTest.Services.Account"));

            services.AddAutoMapper(Assembly.Load("NttDataTest.Services.Account"));

            services.AddValidatorsFromAssembly(Assembly.Load("NttDataTest.Services.Account"));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.Configure<ApiUrls>(opts => Configuration.GetSection("ApiUrls").Bind(opts));

            services.AddHttpClient<IClientProxy, ClientProxy>();
            services.AddHttpClient<ITransactionProxy, TransactionProxy>();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NttDataTest.Api.Account", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NttDataTest.Api.Account v1"));
            }

            var cultureInfo = new CultureInfo("es-ES");

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

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
