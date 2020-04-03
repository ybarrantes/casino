using System.Threading.Tasks;

namespace Casino.Services.Authentication.Contracts
{
    public interface ISignupConfirm
    {
        Task SignUpUserConfirmation(ISignupConfirmModelUser confirmation);
    }
}
