using CatalogApi.Application.Interfaces;
using CatalogApi.Infrastructure.Data;
using CatalogApi.Infrastructure.Repositories;
using Dukkani.Shared.DI;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogApi.Infrastructure.DI
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services,
            IConfiguration config)
        {
            SharedServiceContainer.AddSharedSerives<ProductDbContext>(services, config, config["MySerilog:FileName"]);

            services.AddScoped<IProduct, ProductRepository>();
            return services;
        }
        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            SharedServiceContainer.UseSharedPolicies(app);
            return app;
        }
    }
}
