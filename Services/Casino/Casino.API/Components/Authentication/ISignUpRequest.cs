using System.Threading.Tasks;
using Casino.Data.Models.DTO;

namespace Casino.API.Components.Authentication
{
    public interface ISignUpRequest
    {
        Task<string> SignUpUser(UserSignUpDTO user);
        Task SignUpUserConfirmation(UserConfirmationSignUpDTO confirmation);
    }
}
