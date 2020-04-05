using Casino.Services.Authentication.Contracts;
using Casino.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Test.UnitTests.Config.Mocks
{
    public class AwsCognitoUserGroupsMock : IAwsCognitoUserGroups
    {
        public bool Fail = false;

        public async Task AddUserToGroup(string Username, string GroupName)
        {
            await Task.Run(() => {
                if (Fail)
                    throw new WebApiException(System.Net.HttpStatusCode.InternalServerError);
            });
        }
    }
}
