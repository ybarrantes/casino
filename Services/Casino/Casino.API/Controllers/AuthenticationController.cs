using Casino.Services.WebApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Casino.Data.Context;
using Casino.Data.Models.DTO;
using Casino.Data.Models.Entities;
using Casino.Services.Authentication.Contracts;
using Casino.Services.Authentication.Model;

namespace Casino.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<UsersController> _logger;
        private readonly IAuthentication _authentication;

        public AuthenticationController(IAuthentication authentication, ApplicationDbContext dbContext, ILogger<UsersController> logger)
        {
            _dbContext = dbContext;
            _authentication = authentication;
            _logger = logger;
        }

        [HttpPost("signup")]
        public async Task<ActionResult<WebApiResponse>> SignUp([FromBody] UserSignUpDTO userDTO)
        {
            ISignupModelUser signupModelUser = new SignupModelUser()
            {
                Username = userDTO.Username,
                Password = userDTO.Password,
                Email = userDTO.Email,
                Name = userDTO.Name,
                MiddleName = userDTO.MiddleName,
                BirthDate = userDTO.BirthDate
            };

            string cloudIdentityId = await _authentication.SignUpUser(signupModelUser);

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
            ISignupConfirmModelUser signupConfirm = new SignupConfirmModelUser(confirmation.Username, confirmation.ConfirmationCode);

            await _authentication.SignUpUserConfirmation(signupConfirm);

            return new WebApiResponse().Success();
        }

        [HttpPost("signin")]
        public async Task<ActionResult<WebApiResponse>> SignIn([FromBody] UserSignInDTO userDTO)
        {
            ISigninModelUser signinModelUser = new SigninModelUser(userDTO.Username, userDTO.Password);
            
            ISigninModelResponse signinModelResponse = await _authentication.SignInUser(signinModelUser);
            
            return new WebApiResponse().Success().SetData(signinModelResponse);
        }
    }
}
