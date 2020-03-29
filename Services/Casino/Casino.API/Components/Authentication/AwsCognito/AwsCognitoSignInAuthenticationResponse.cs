using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Casino.API.Exceptions;
using System;

namespace Casino.API.Components.Authentication.AwsCognito
{
    public class AwsCognitoSignInAuthenticationResponse : ISignInResponse
    {
        protected string _accessToken;
        protected AdminInitiateAuthResponse adminInitiateAuthResponse;

        public string AccessToken { get => _accessToken; }

        public AwsCognitoSignInAuthenticationResponse(AdminInitiateAuthResponse authResponse)
        {
            adminInitiateAuthResponse = authResponse;
            ValidateAdminInitiateAuthResponse();

            _accessToken = adminInitiateAuthResponse.AuthenticationResult.AccessToken;
        }

        private void ValidateAdminInitiateAuthResponse()
        {
            if (adminInitiateAuthResponse == null || adminInitiateAuthResponse.AuthenticationResult == null)
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError, "Aws Cognito Service Error");

            if (adminInitiateAuthResponse.ChallengeName == ChallengeNameType.NEW_PASSWORD_REQUIRED)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Forbidden, "Please, change password");
            }
        }
    }
}
