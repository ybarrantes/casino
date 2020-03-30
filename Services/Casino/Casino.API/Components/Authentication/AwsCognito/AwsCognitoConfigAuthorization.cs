using Casino.API.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Components.Authentication.AwsCognito
{
    public class AwsCognitoConfigAuthorization : AwsCognitoAuthenticationBase, IConfigAuthorization
    {
        public AwsCognitoConfigAuthorization(IConfiguration configuration, ILogger logger)
            : base(configuration, logger)
        {
        }

        public AuthorizationOptions GetAuthorizationOptions(AuthorizationOptions options)
        {
            List<string> groups = GetAuthorizedGroups();
            
            if(groups.Count > 0)
            {
                foreach(string group in groups)
                {
                    options.AddPolicy(group, policy => policy.RequireClaim("cognito:groups", new List<string> { group }));
                }
            }

            return options;
        }
    }
}
