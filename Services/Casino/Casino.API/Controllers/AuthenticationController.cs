using Casino.Services.WebApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Casino.Data.Context;
using Casino.Data.Models.DTO.Users;
using Casino.Data.Models.Entities;
using Casino.Services.Authentication.Contracts;
using Casino.Services.Authentication.Model;
using Casino.Services.DB.SQL.Crud;

namespace Casino.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IAuthentication _authentication;
        private readonly ISqlContextCrud<User> _contextCRUD;

        public AuthenticationController(
            ApplicationDbContext dbContext,
            IAuthentication authentication,
            ILogger<UsersController> logger,
            ISqlContextCrud<User> contextCRUD)
        {
            _authentication = authentication;
            _logger = logger;
            _contextCRUD = contextCRUD;

            _contextCRUD.AppDbContext = dbContext;
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

                await _contextCRUD.CreateFromEntityAsync(userEntity);

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
