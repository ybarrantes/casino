using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;

namespace Casino.API.Config
{
    static class ConfigureAuthenticationService
    {
        public static void AddDependencies(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.Audience = configuration["AWS:Cognito:AppClientId"];
                    options.MetadataAddress = configuration["AWS:Cognito:MetadataAddress"];
                    options.IncludeErrorDetails = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        LifetimeValidator = (before, expires, token, param) => expires > DateTime.UtcNow,
                    };
                });
        }
    }
}
