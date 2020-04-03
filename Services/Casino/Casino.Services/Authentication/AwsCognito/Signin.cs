using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using System.Threading.Tasks;
using System;
using Casino.Services.WebApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Casino.Services.Authentication.Contracts;

namespace Casino.Services.Authentication.AwsCognito
{
    public class Signin : AuthenticationBase, ISignin
    {
        public Signin(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ISigninModelResponse> SignInUser(ISigninModelUser user)
        {
            AdminInitiateAuthResponse authResponse = await TrySignInUser(user);

            return new SigninResponse(authResponse);            
        }

        private async Task<AdminInitiateAuthResponse> TrySignInUser(ISigninModelUser user)
        {
            try
            {
                AdminInitiateAuthRequest authRequest = GetAuthRequest(user.Username, user.Password);

                AmazonCognitoIdentityProviderClient identityProviderClient = GetAmazonCognitoIdentity();

                return await identityProviderClient.AdminInitiateAuthAsync(authRequest);
            }
            catch (NotAuthorizedException e)
            {
                throw new WebApiException(System.Net.HttpStatusCode.Unauthorized, e.Message);
            }
            catch (UserNotConfirmedException e)
            {
                throw new WebApiException(System.Net.HttpStatusCode.Forbidden, e.Message);
            }
            catch (Exception e)
            {
                throw new WebApiException(System.Net.HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private AdminInitiateAuthRequest GetAuthRequest(string username, string password)
        {
            AdminInitiateAuthRequest request = new AdminInitiateAuthRequest
            {
                UserPoolId = GetUserPoolId(),
                ClientId = GetClientId(),
                AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH
            };

            request.AuthParameters.Add(AuthenticationParameters.USERNAME, username);
            request.AuthParameters.Add(AuthenticationParameters.PASSWORD, password);

            return request;
        }
    }
}
