
namespace Casino.Services.Authentication.Contracts
{
    public interface ISigninModelResponse
    {
        string AccessToken { get; }
        string TokenType { get; }
        string RefreshToken { get; }
        string IdToken { get; }
        int ExpiresIn { get; }
    }
}
