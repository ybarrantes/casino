using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Casino.API.Exceptions;
using System;

namespace Casino.API.Components.Authentication.AwsCognito
{
    public class AwsCognitoSignInAuthenticationResponse : ISignInResponse
    {
        protected string _accessToken;
        protected string _idToken;
        protected string _tokenType;
        protected string _refreshToken;
        protected int _expiresIn;
        protected AdminInitiateAuthResponse _adminInitiateAuthResponse;

        public string AccessToken { get => _accessToken; }
        public string IdToken { get => _idToken; }
        public string TokenType { get => _tokenType; }
        public string RefreshToken { get => _refreshToken; }
        public int ExpiresIn { get => _expiresIn; }

        public AwsCognitoSignInAuthenticationResponse(AdminInitiateAuthResponse authResponse)
        {
            _adminInitiateAuthResponse = authResponse;
            ValidateAdminInitiateAuthResponse();

            _accessToken = _adminInitiateAuthResponse.AuthenticationResult.AccessToken;
            _idToken = _adminInitiateAuthResponse.AuthenticationResult.IdToken;
            _tokenType = _adminInitiateAuthResponse.AuthenticationResult.TokenType;
            _refreshToken = _adminInitiateAuthResponse.AuthenticationResult.RefreshToken;
            _expiresIn = _adminInitiateAuthResponse.AuthenticationResult.ExpiresIn;
        }

        private void ValidateAdminInitiateAuthResponse()
        {
            if (_adminInitiateAuthResponse == null || _adminInitiateAuthResponse.AuthenticationResult == null)
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError, "Aws Cognito Service Error");

            // TODO: validate other cases of ChallengeName
            if (_adminInitiateAuthResponse.ChallengeName == ChallengeNameType.NEW_PASSWORD_REQUIRED)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Forbidden, "Please, change password");
            }
        }
    }
}
