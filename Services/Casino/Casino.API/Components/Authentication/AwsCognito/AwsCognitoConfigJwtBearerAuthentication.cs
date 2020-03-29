using Casino.API.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Components.Authentication.AwsCognito
{
    public class AwsCognitoConfigJwtBearerAuthentication : AwsCognitoAuthenticationBase, IConfigJwtBearerAuthentication
    {
        public JwtBearerOptions GetJwtBearerAuthenticationOptions(JwtBearerOptions options)
        {
            options.SaveToken = true;
            options.Audience = GetClientId();
            options.MetadataAddress = ApiConfig.Singleton.Configuration["AWS:Cognito:MetadataAddress"];
            options.IncludeErrorDetails = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                LifetimeValidator = (before, expires, token, param) => expires > DateTime.UtcNow,
            };

            return options;
        }
    }
}
