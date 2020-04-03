using Casino.Services.Authentication.Contracts;
using Casino.Services.WebApi;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Casino.IntegrationTests.Authentication
{
    public class AuthenticationSuccessMock : IAuthentication
    {
        public async Task<ISigninModelResponse> SignInUser(ISigninModelUser user)
        {
            return await Task.Run(() => {
                return new SigninModelResponseMock(
                    "asdfg",
                    "asdfg",
                    "asdfg",
                    "asdfg",
                    12154587);
            });
        }

        public async Task<string> SignUpUser(ISignupModelUser user)
        {
            return await Task.Run(() => "123456");
        }

        public async Task SignUpUserConfirmation(ISignupConfirmModelUser confirmation)
        {
            await Task.Run(() => true);
        }
    }
}
