using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Casino.API.Config
{
    public static class ConfigureAuthorizationService
    {
        public static void AddDependencies(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthorization(options =>
                {
                    List<string> groups = configuration
                        .GetSection("AWS:Cognito:AuthorizedGroups")
                        .Get<List<string>>();

                    foreach (string group in groups)
                    {
                        options.AddPolicy(group, policy =>
                            policy.RequireClaim("cognito:groups", new List<string> { group }));
                    }
                });
        }
    }
}
