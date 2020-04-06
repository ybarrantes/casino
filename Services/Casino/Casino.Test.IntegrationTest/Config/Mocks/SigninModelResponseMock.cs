using Casino.Services.Authentication.Contracts;

namespace Casino.Test.IntegrationTest.Config.Mocks
{
    public class SigninModelResponseMock : ISigninModelResponse
    {
        public string AccessToken { get; }

        public string TokenType { get; }

        public string RefreshToken { get; }

        public string IdToken { get; }

        public int ExpiresIn { get; }

        public SigninModelResponseMock(string accessToken, string tokenType, string refreshToken, string idToken, int expiresIn)
        {
            AccessToken = accessToken;
            TokenType = tokenType;
            RefreshToken = refreshToken;
            IdToken = idToken;
            ExpiresIn = expiresIn;
        }
    }
}
