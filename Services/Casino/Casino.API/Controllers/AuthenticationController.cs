using Casino.API.Components.Authentication;
using Casino.API.Components.Authentication.AwsCognito;
using Casino.API.Data.Context;
using Casino.API.Data.Entities;
using Casino.API.Data.Models.Usuario;
using Casino.API.Exceptions;
using Casino.API.Util.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IConfiguration configuration;
        private readonly ILogger<AuthenticationController> logger;

        public AuthenticationController(ApplicationDbContext dbContext, IConfiguration configuration, ILogger<AuthenticationController> logger)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
            this.logger = logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult<HttpResponse>> SignUp([FromBody] UsuarioSignUpDTO userDTO)
        {
            ISignUpRequest request = new AwsCognitoSignUpAuthentication(configuration, logger);
            string userSub = await request.SignUpUser(userDTO);

            await TrySaveUserInLocalDB(userDTO, userSub);

            return new HttpResponse().Success();
        }

        private async Task TrySaveUserInLocalDB(UsuarioSignUpDTO userDTO, string userSub)
        {
            try
            {
                Usuario userEntity = new Usuario()
                {
                    Username = userDTO.Username,
                    Email = userDTO.Email,
                    CloudIdentityId = userSub,
                };

                dbContext.Usuarios.Add(userEntity);
                await dbContext.SaveChangesAsync();

                logger.LogInformation($"user '{userDTO.Username}' has been saved in local db");
            }
            catch (Exception e)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost("register-confirmation")]
        public async Task<ActionResult<HttpResponse>> SignIn([FromBody] UsuarioConfirmationSignUpDTO confirmation)
        {
            ISignUpRequest confirmRequest = new AwsCognitoSignUpAuthentication(configuration, logger);
            await confirmRequest.SignUpUserConfirmation(confirmation);
            return new HttpResponse().Success();
        }

        [HttpPost("signin")]
        public async Task<ActionResult<HttpResponse>> SignIn([FromBody] UsuarioSignInDTO userDTO)
        {
            ISignInRequest authRequest = new AwsCognitoSignInAuthentication(configuration, logger);
            ISignInResponse authResponse = await authRequest.SignInUser(userDTO);
            return new HttpResponse().Success().SetData(authResponse);
        }
    }
}
