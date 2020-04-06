using Casino.Services.WebApi;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Casino.Data.Context;
using Casino.Data.Models.DTO.Users;
using Casino.Data.Models.Entities;
using Casino.Services.Authentication.Contracts;
using Casino.Services.Authentication.Model;
using Casino.Services.DB.SQL.Crud;
using Casino.API.Components.Users;

namespace Casino.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthentication _authentication;
        private readonly ISqlContextCrud<User> _contextCRUD;

        public AuthenticationController(
            ApplicationDbContext dbContext,
            IAuthentication authentication,
            ISqlContextCrud<User> contextCRUD)
        {
            _authentication = authentication;
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
                MiddleName = userDTO.MiddleName
            };

            string cloudIdentityId = await _authentication.SignUpUser(signupModelUser);

            return await ((IUserComponent)_contextCRUD).CreateUserAndUserAccountsAsync(userDTO, cloudIdentityId);
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
