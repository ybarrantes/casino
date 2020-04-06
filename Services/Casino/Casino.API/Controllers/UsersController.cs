using Casino.Services.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Casino.Data.Models.DTO.Users;
using Casino.Data.Models.Entities;
using Casino.Services.Authentication.Contracts;
using Casino.Services.DB.SQL.Crud;
using Casino.Data.Context;

namespace Casino.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAwsCognitoUserGroups _cognitoUserGroups;
        private readonly ISqlContextCrud<User> _crudComponent;

        public UsersController(
            ApplicationDbContext dbContext,
            IConfiguration configuration,
            IAwsCognitoUserGroups cognitoUserGroups,
            ISqlContextCrud<User> crudComponent)
        {
            _configuration = configuration;
            _cognitoUserGroups = cognitoUserGroups;

            _crudComponent = crudComponent;
            _crudComponent.AppDbContext = dbContext;
        }

        [HttpGet]
        [Authorize]
        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "SuperAdmin")]
        [Authorize(Policy = "SystemManager")]
        public async Task<ActionResult<WebApiResponse>> GetAll(int page = 1)
        {
            return await _crudComponent.GetAllPagedRecordsAndMakeResponseAsync(page, 20);
        }

        [Authorize]
        [HttpGet("{userId}")]
        public async Task<ActionResult<WebApiResponse>> GetUser(long userId)
        {
            // TODO: pending validate policies, if rol is player,
            // can only see its own user, other roles can see all users
            return await _crudComponent.FirstByIdAndMakeResponseAsync(userId);
        }

        [HttpPost("{userId}/roles")]
        [Authorize(Policy = "SuperAdmin")]
        [Authorize(Policy = "SystemManager")]
        public async Task<ActionResult<WebApiResponse>> AddRole(long userId, [FromBody] UserRoleDTO role)
        {
            if (!CheckRoleIsAuthorized(role.Role))
                throw new WebApiException(System.Net.HttpStatusCode.BadRequest, $"The role '{role.Role}' is not authorized in aws cognito groups, see configuration");

            User user = await _crudComponent.FirstByIdAsync(userId);

            await _cognitoUserGroups.AddUserToGroup(user.Username, role.Role);

            return new WebApiResponse().Success();
        }

        private bool CheckRoleIsAuthorized(string role)
        {
            List<string> roles = _configuration.GetSection("AWS:Cognito:AuthorizedGroups").Get<List<string>>();

            return roles.Contains(role);
        }
    }
}
