using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Casino.API.Data.Models.Usuario;
using System.Threading.Tasks;
using System;
using Casino.API.Exceptions;

namespace Casino.API.Components.Authentication.AwsCognito
{
    public class AwsCognitoSignInAuthentication : AwsCognitoAuthenticationBase, ISignInRequest
    {

        async Task<ISignInResponse> ISignInRequest.SignInUser(UsuarioSignInDTO user)
        {
            AdminInitiateAuthResponse authResponse = null;

            try
            {
                AdminInitiateAuthRequest authRequest = GetAuthRequest(user.Username, user.Password);
                authResponse = await GetAmazonCognitoIdentity().AdminInitiateAuthAsync(authRequest);
            }
            catch (NotAuthorizedException e)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized, e.Message);
            }
            catch (UserNotConfirmedException e)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Forbidden, e.Message);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError, e.Message);
            }

            return new AwsCognitoSignInAuthenticationResponse(authResponse);            
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
