using Microsoft.OpenApi.Models;
using Casino.API.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Casino.Data.Context;
using Swashbuckle.AspNetCore.Swagger;

namespace Casino.API.Config
{
    public static class InversionOfControl
    {
        private static IServiceCollection _services = null;
        private static IConfiguration _configuration = null;

        public static IConfiguration Configuration => _configuration;

        public static void AddDependencies(IServiceCollection services, IConfiguration configuration)
        {
            if (_services != null)
                throw new InvalidOperationException("The services configuration has already been applied");

            _services = services;
            _configuration = configuration;

            AddSwaggerToServicesContainer();

            _services.AddHttpContextAccessor();

            AddControllersToServicesContainer();

            ConfigureAutoMapperService.AddDependencies(services, configuration);

            AddDbContextToServicesContainer();

            ConfigureAuthenticationService.AddDependencies(services, configuration);

            ConfigureAuthorizationService.AddDependencies(services, configuration);

            ConfigureCustomServices.AddDependencies(services, configuration);
        }

        private static void AddSwaggerToServicesContainer()
        {
            _services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo { Title = "Casino", Version = "v1" });
            });
        }

        private static void AddControllersToServicesContainer()
        {
            _services.AddControllers(
                    options => options.Filters.Add(typeof(ApiExceptionFilter))
                )
                .AddNewtonsoftJson(
                    options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
        }

        private static void AddDbContextToServicesContainer()
        {
            _services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")),
                ServiceLifetime.Transient
            );
        }
    }
}
