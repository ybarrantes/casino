using Casino.Services.Authentication.Contracts;

namespace Casino.Services.Authentication.Model
{
    public class SignupConfirmModelUser : ISignupConfirmModelUser
    {
        public string Username { get; set; }

        public string ConfirmationCode { get; set; }

        public SignupConfirmModelUser(string username, string confirmationCode)
        {
            Username = username;
            ConfirmationCode = confirmationCode;
        }
    }
}
