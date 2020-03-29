using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Casino.API.Util.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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

            logger.LogInformation($"add user '{Username}' to group '{GroupName}' successful. [{response.HttpStatusCode.ToString()}]");

            return response;
        }
    }
}
