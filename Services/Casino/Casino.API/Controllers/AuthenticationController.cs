using Casino.API.Components.Authentication;
using Casino.API.Components.Authentication.AwsCognito;
using Casino.API.Data.Models.Usuario;
using Casino.API.Exceptions;
using Casino.API.Util.Logging;
using Casino.API.Util.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API
{
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        [HttpPost]
        [Route("api/auth/register")]
        public async Task<ActionResult<HttpResponse>> SignUp(UsuarioSignUpDTO user)
        {
            ISignUpRequest request = new AwsCognitoSignUpAuthentication();
            await request.SignUpUser(user);
            return new HttpResponse().Success();
        }

        [HttpPost]
        [Route("api/auth/register-confirmation")]
        public async Task<ActionResult<HttpResponse>> SignIn(UsuarioConfirmationSignUpDTO confirmation)
        {
            ISignUpRequest confirmRequest = new AwsCognitoSignUpAuthentication();
            await confirmRequest.SignUpUserConfirmation(confirmation);
            return new HttpResponse().Success();
        }

        [HttpPost]
        [Route("api/auth/signin")]
        public async Task<ActionResult<HttpResponse>> SignIn(UsuarioSignInDTO user)
        {
            ISignInRequest authRequest = new AwsCognitoSignInAuthentication();
            ISignInResponse authResponse = await authRequest.SignInUser(user);
            return new HttpResponse().Success().SetData(authResponse);
        }
    }
}
