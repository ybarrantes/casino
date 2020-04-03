
namespace Casino.Services.Authentication.Contracts
{
    public interface ISigninModelUser
    {
        string Username { get; set; }

        string Password { get; set; }
    }
}
