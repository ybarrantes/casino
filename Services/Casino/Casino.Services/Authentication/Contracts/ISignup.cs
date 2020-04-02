using System.Threading.Tasks;

namespace Casino.Services.Authentication.Contracts
{
    public interface ISignup
    {
        Task<string> SignUpUser(ISignupModelUser user);
    }
}
