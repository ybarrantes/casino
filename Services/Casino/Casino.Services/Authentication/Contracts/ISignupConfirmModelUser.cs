
namespace Casino.Services.Authentication.Contracts
{
    public interface ISignupConfirmModelUser
    {
        string Username { get; set; }

        string ConfirmationCode { get; set; }
    }
}
