using System.Threading.Tasks;
using Casino.Data.Models.DTO;

namespace Casino.API.Components.Authentication
{
    public interface ISignInRequest
    {
        Task<ISignInResponse> SignInUser(UserSignInDTO user);
    }
}
