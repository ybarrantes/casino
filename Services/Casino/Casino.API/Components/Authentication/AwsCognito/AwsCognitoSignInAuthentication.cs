using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using System.Threading.Tasks;
using System;
using Casino.Services.WebApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Casino.Data.Models.DTO;

namespace Casino.API.Components.Authentication.AwsCognito
{
    public class AwsCognitoSignInAuthentication : AwsCognitoAuthenticationBase, ISignInRequest
    {
        public AwsCognitoSignInAuthentication(IConfiguration configuration, ILogger logger)
            : base(configuration, logger)
        {
        }

        async Task<ISignInResponse> ISignInRequest.SignInUser(UserSignInDTO user)
        {
            AdminInitiateAuthResponse authResponse = await TrySignInUser(user);

            return new AwsCognitoSignInAuthenticationResponse(authResponse);            
        }

        private async Task<AdminInitiateAuthResponse> TrySignInUser(UserSignInDTO user)
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

            request.AuthParameters.Add(AwsCognitoAuthenticationParameters.USERNAME, username);
            request.AuthParameters.Add(AwsCognitoAuthenticationParameters.PASSWORD, password);

            return request;
        }
    }
}
