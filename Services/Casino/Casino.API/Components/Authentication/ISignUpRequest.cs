using Casino.API.Data.Models.Usuario;
using System.Threading.Tasks;

namespace Casino.API.Components.Authentication
{
    interface ISignUpRequest
    {
        public abstract Task SignUpUser(UsuarioSignUpDTO user);
        public abstract Task SignUpUserConfirmation(UsuarioConfirmationSignUpDTO confirmation);
    }
}
