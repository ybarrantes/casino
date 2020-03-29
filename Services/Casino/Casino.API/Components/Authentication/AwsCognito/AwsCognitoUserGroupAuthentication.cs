using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Casino.API.Config;
using Casino.API.Util.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Components.Authentication.AwsCognito
{
    public class AwsCognitoUserGroupAuthentication : AwsCognitoAuthenticationBase
    {
        public async Task<AdminAddUserToGroupResponse> AddUserToGroup(string Username, string GroupName)
        {
            AdminAddUserToGroupRequest request = new AdminAddUserToGroupRequest()
            {
                GroupName = GroupName,
                Username = Username,
                UserPoolId = GetUserPoolId(),
            };
            AmazonCognitoIdentityProviderClient client = GetAmazonCognitoIdentity();
            AdminAddUserToGroupResponse response = await client.AdminAddUserToGroupAsync(request);

            Logger.Info($"add user '{Username}' to group '{GroupName}' successful");

            return response;
        }
    }
}
