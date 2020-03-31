using Casino.API.Data.Models.User;
using System.Threading.Tasks;

namespace Casino.API.Components.Authentication
{
    public interface ISignInRequest
    {
        Task<ISignInResponse> SignInUser(UserSignInDTO user);
    }
}
