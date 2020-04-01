using Casino.API.Components.Authentication;
using Casino.API.Components.Authentication.AwsCognito;
using Casino.Services.WebApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Casino.Data.Context;
using Casino.Data.Models.DTO;
using Casino.Data.Models.Entities;

namespace Casino.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(ApplicationDbContext dbContext, IConfiguration configuration, ILogger<AuthenticationController> logger)
        {
            this._dbContext = dbContext;
            this._configuration = configuration;
            this._logger = logger;
        }

        [HttpPost("signup")]
        public async Task<ActionResult<WebApiResponse>> SignUp([FromBody] UserSignUpDTO userDTO)
        {
            ISignUpRequest request = new AwsCognitoSignUpAuthentication(_configuration, _logger);
            string cloudIdentityId = await request.SignUpUser(userDTO);

            await TrySaveUserInLocalDB(userDTO, cloudIdentityId);

            return new WebApiResponse().Success();
        }

        private async Task TrySaveUserInLocalDB(UserSignUpDTO userDTO, string cloudIdentityId)
        {
            try
            {
                User userEntity = new User()
                {
                    Username = userDTO.Username,
                    Email = userDTO.Email,
                    CloudIdentityId = cloudIdentityId,
                };

                _dbContext.Users.Add(userEntity);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"user '{userDTO.Username}' has been saved in local db");
            }
            catch (Exception e)
            {
                throw new WebApiException(System.Net.HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost("signup/confirmation")]
        public async Task<ActionResult<WebApiResponse>> SignIn([FromBody] UserConfirmationSignUpDTO confirmation)
        {
            ISignUpRequest confirmRequest = new AwsCognitoSignUpAuthentication(_configuration, _logger);
            await confirmRequest.SignUpUserConfirmation(confirmation);
            return new WebApiResponse().Success();
        }

        [HttpPost("signin")]
        public async Task<ActionResult<WebApiResponse>> SignIn([FromBody] UserSignInDTO userDTO)
        {
            ISignInRequest authRequest = new AwsCognitoSignInAuthentication(_configuration, _logger);
            ISignInResponse authResponse = await authRequest.SignInUser(userDTO);
            return new WebApiResponse().Success().SetData(authResponse);
        }
    }
}
