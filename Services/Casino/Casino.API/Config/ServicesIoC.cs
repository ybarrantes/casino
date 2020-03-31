using AutoMapper;
using Casino.API.Data.Context;
using Casino.API.Data.Entities;
using Casino.API.Data.Models.Dominio;
using Casino.API.Data.Models.Ruleta;
using Casino.API.Services;
using Casino.API.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Casino.API.Components.Ruletas;

namespace Casino.API.Config
{
    public static class ServicesIoC
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

            // app services
            _services.AddHttpContextAccessor();
            AddControllersToServicesContainer();
            AddAutoMapperToServicesContainer();
            AddDbContextToServicesContainer();
            AddAuthenticationToServicesContainer();
            AddAuthorizationToServicesContainer();
            AddIdentityToServicesContainer();
            _services.AddMvc();

            // custom services
            _services.AddScoped<IIdentityApp, IdentityAppService>();
            _services.AddScoped<IRuletasComponent, RuletasComponent>();
        }

        private static void AddControllersToServicesContainer()
        {
            _services.AddControllers(
                    options => options.Filters.Add(new HttpResponseExceptionFilter())
                )
                .AddNewtonsoftJson(
                    options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
        }

        private static void AddAutoMapperToServicesContainer()
        {
            _services.AddAutoMapper(config =>
            {
                config.CreateMap<Ruleta, RuletaCreateDTO>();
                config.CreateMap<RuletaCreateDTO, Ruleta>();
                config.CreateMap<Ruleta, RuletaShowDTO>();
                //config.CreateMap<RuletaShowDTO, Ruleta>();

                config.CreateMap<Dominio, DominioShowDTO>();
                config.CreateMap<DominioShowDTO, Dominio>();
            }, typeof(Startup));
        }

        private static void AddDbContextToServicesContainer()
        {
            _services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")),
                ServiceLifetime.Transient
            );
        }

        private static void AddAuthenticationToServicesContainer()
        {
            // TODO: mejorar a singleton
            _services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.Audience = _configuration["AWS:Cognito:AppClientId"];
                    options.MetadataAddress = _configuration["AWS:Cognito:MetadataAddress"];
                    options.IncludeErrorDetails = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        LifetimeValidator = (before, expires, token, param) => expires > DateTime.UtcNow,
                    };
                });
        }

        private static void AddAuthorizationToServicesContainer()
        {
            // TODO: mejorar a singleton
            _services
                .AddAuthorization(options =>
                {
                    List<string> groups = _configuration.GetSection("AWS:Cognito:AuthorizedGroups").Get<List<string>>();

                    foreach (string group in groups)
                    {
                        options.AddPolicy(group, policy => 
                            policy.RequireClaim("cognito:groups", new List<string> { group }));
                    }
                });
        }

        private static void AddIdentityToServicesContainer()
        {

        }
    }
}
