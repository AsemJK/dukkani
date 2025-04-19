using Dukkani.Shared.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Dukkani.Shared.DI
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedSerives<TContext>(this IServiceCollection services,
            IConfiguration config,
            string fileName) where TContext : DbContext
        {
            services.AddDbContext<TContext>(option => option.UseSqlServer(
                config.GetConnectionString("eCommerceConnectionString"), sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()
                ));
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(path: $"{fileName}.text",
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}{Level:u3}]{Message:lj}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day
                ).CreateLogger();

            JwtAuthenticationScheme.AddJWTAuthenticationScheme(services, config);

            return services;
        }
        public static IApplicationBuilder UseSharedPolicies(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalException>();
            app.UseMiddleware<ListenToApiGateway>();
            return app;
        }
    }
}
