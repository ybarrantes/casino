using System.Threading.Tasks;

namespace Casino.Services.Authentication.Contracts
{
    public interface ISignin
    {
        Task<ISigninModelResponse> SignInUser(ISigninModelUser user);
    }
}
