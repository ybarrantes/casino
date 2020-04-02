using AutoMapper;
using Casino.API.Services;
using Casino.API.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using Casino.Data.Context;
using Casino.Data.Extension;
using Casino.Services.WebApi;
using Casino.Services.DB.SQL;
using Casino.Services.DB.SQL.Contracts.CRUD;
using Casino.Services.Authentication.Contracts;
using Casino.Services.Authentication;
using Casino.Data.Models.Entities;
using Casino.API.Components;

namespace Casino.API.Config
{
    public static class IoC
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
            _services.AddAutoMapper(config => MapModels.GetMapper(config), typeof(Startup));
            AddDbContextToServicesContainer();
            AddAuthenticationToServicesContainer();
            AddAuthorizationToServicesContainer();

            // custom services
            _services.AddScoped(typeof(IAuthentication), typeof(AwsCognitoAuthentication));
            _services.AddScoped(typeof(IIdentityApp<>), typeof(IdentityApp<>));
            _services.AddScoped(typeof(IContextCRUD<>), typeof(ContextCRUD<>));
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
    }
}
