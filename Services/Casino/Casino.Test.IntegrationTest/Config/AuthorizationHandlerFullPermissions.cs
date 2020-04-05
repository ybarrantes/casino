using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Casino.Test.IntegrationTest.Config
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
