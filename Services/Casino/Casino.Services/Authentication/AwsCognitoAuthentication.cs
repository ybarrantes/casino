using Casino.Services.Authentication.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Casino.Services.Authentication.AwsCognito;

namespace Casino.Services.Authentication
{
    public class AwsCognitoAuthentication : IAuthentication
    {
        private readonly ISignin _signin = null;
        private readonly ISignup _signup = null;
        private readonly ISignupConfirm _signupConfirm = null;

        public AwsCognitoAuthentication(IConfiguration configuration)
        {
            _signin = new Signin(configuration);
            _signup = new Signup(configuration);
            _signupConfirm = new SignupConfirm(configuration);
        }

        public async Task<ISigninModelResponse> SignInUser(ISigninModelUser user)
        {
            return await _signin.SignInUser(user);
        }

        public async Task<string> SignUpUser(ISignupModelUser user)
        {
            return await _signup.SignUpUser(user);
        }

        public async Task SignUpUserConfirmation(ISignupConfirmModelUser confirmation)
        {
            await _signupConfirm.SignUpUserConfirmation(confirmation);
        }
    }
}
