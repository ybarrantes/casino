
namespace Casino.API.Components.Authentication
{
    public interface ISignInResponse
    {
        public string AccessToken { get; }
        public string TokenType { get; }
        public string RefreshToken { get; }
        public string IdToken { get; }
        public int ExpiresIn { get; }
    }
}
