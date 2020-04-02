using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Casino.Services.Authentication.Contracts;
using Casino.Services.WebApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Casino.Services.Authentication.AwsCognito
{
    public class SignupConfirm : AuthenticationBase, ISignupConfirm
    {
        public SignupConfirm(IConfiguration configuration)
           : base(configuration)
        {
        }

        public async Task SignUpUserConfirmation(ISignupConfirmModelUser confirmation)
        {
            try
            {
                ConfirmSignUpRequest confirmRequest = new ConfirmSignUpRequest()
                {
                    Username = confirmation.Username,
                    ConfirmationCode = confirmation.ConfirmationCode,
                    ClientId = GetClientId(),
                };

                AmazonCognitoIdentityProviderClient client = GetAmazonCognitoIdentity();
                ConfirmSignUpResponse confirmResult = await client.ConfirmSignUpAsync(confirmRequest);
            }
            catch (CodeMismatchException e)
            {
                throw new WebApiException(System.Net.HttpStatusCode.Forbidden, e.Message);
            }
            catch (ExpiredCodeException e)
            {
                throw new WebApiException(System.Net.HttpStatusCode.Forbidden, e.Message);
            }
            catch (Exception e)
            {
                throw new WebApiException(System.Net.HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
