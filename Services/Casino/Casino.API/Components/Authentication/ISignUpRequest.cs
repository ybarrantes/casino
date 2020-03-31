using Casino.API.Data.Models.User;
using System.Threading.Tasks;

namespace Casino.API.Components.Authentication
{
    public interface ISignUpRequest
    {
        Task<string> SignUpUser(UserSignUpDTO user);
        Task SignUpUserConfirmation(UserConfirmationSignUpDTO confirmation);
    }
}
