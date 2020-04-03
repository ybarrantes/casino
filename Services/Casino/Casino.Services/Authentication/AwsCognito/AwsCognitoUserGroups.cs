using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Casino.Services.Authentication.Contracts;
using Casino.Services.WebApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace Casino.Services.Authentication.AwsCognito
{
    public class AwsCognitoUserGroups : AuthenticationBase, IAwsCognitoUserGroups
    {
        public AwsCognitoUserGroups(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task AddUserToGroup(string Username, string GroupName)
        {
            AdminAddUserToGroupRequest request = new AdminAddUserToGroupRequest()
            {
                GroupName = GroupName,
                Username = Username,
                UserPoolId = GetUserPoolId(),
            };

            AmazonCognitoIdentityProviderClient identityProvider = GetAmazonCognitoIdentity();
            AdminAddUserToGroupResponse response = await identityProvider.AdminAddUserToGroupAsync(request);

            if (!response.HttpStatusCode.Equals(HttpStatusCode.OK))
                throw new WebApiException(response.HttpStatusCode, "add user to group failed");
        }
    }
}
