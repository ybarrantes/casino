using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Casino.IntegrationTests.Config
{
    public class AuthorizationHandlerFullPermissions : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            foreach(var requirement in context.Requirements)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
