using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Casino.Services.Authentication.AwsCognito
{
    public class AwsCognitoUserGroupAuthentication : AuthenticationBase
    {
        public AwsCognitoUserGroupAuthentication(IConfiguration configuration)
            : base(configuration)
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

            return response;
        }
    }
}
