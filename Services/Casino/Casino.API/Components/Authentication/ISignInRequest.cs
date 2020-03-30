using Casino.API.Data.Models.Usuario;
using System.Threading.Tasks;

namespace Casino.API.Components.Authentication
{
    interface ISignInRequest
    {
        public abstract Task<ISignInResponse> SignInUser(UsuarioSignInDTO user);
    }
}
