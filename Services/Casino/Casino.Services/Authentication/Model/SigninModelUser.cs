using Casino.Services.Authentication.Contracts;

namespace Casino.Services.Authentication.Model
{
    public class SigninModelUser : ISigninModelUser
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public SigninModelUser(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
