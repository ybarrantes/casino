using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Casino.API.Components.Authentication.AwsCognito
{
    public class AwsCognitoUserGroupAuthentication : AwsCognitoAuthenticationBase
    {
        public AwsCognitoUserGroupAuthentication(IConfiguration configuration, ILogger logger)
            : base(configuration, logger)
        {
        }

        public async Task<AdminAddUserToGroupResponse> AddUserToGroup(string Username, string GroupName)
        {
            AdminAddUserToGroupRequest request = new AdminAddUserToGroupRequest()
            {
                GroupName = GroupName,
                Username = Username,
                UserPoolId = GetUserPoolId(),
            };

            AmazonCognitoIdentityProviderClient identityProvider = GetAmazonCognitoIdentity();
            AdminAddUserToGroupResponse response = await identityProvider.AdminAddUserToGroupAsync(request);

            _logger.LogInformation($"Add user '{Username}' to group '{GroupName}' successful. [{response.HttpStatusCode}]");

            return response;
        }
    }
}
