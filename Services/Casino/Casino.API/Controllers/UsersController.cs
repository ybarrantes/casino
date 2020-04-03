using Casino.Services.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Casino.Data.Context;
using Casino.Data.Models.DTO;
using Casino.Data.Models.Entities;
using Casino.Services.Authentication.AwsCognito;
using Casino.Services.Authentication.Contracts;

namespace Casino.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IAwsCognitoUserGroups _cognitoUserGroups;

        public UsersController(
            ApplicationDbContext dbContext,
            IConfiguration configuration,
            IAwsCognitoUserGroups cognitoUserGroups)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _cognitoUserGroups = cognitoUserGroups;
        }

        [Authorize]
        [HttpGet("{id}", Name = "GetUsuario")]
        public async Task<ActionResult<WebApiResponse>> GetUser(long id)
        {
            User user = await FindUsuarioById(id);
            return new WebApiResponse().Success().SetData(user);
        }

        private async Task<User> FindUsuarioById(long id)
        {
            User user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id.Equals(id));

            if(user == null)
                throw new WebApiException(System.Net.HttpStatusCode.NotFound, $"User '{id}' not found");

            return user;
        }

        [Authorize(Policy = "SuperAdmin")]
        [HttpPost("{id}/roles")]
        public async Task<ActionResult<WebApiResponse>> AddRole(long id, [FromBody] UserRoleDTO role)
        {
            if (!CheckRoleIsAuthorized(role.Role))
                throw new WebApiException(System.Net.HttpStatusCode.BadRequest, $"The role '{role.Role}' is not authorized in aws cognito groups, see configuration");

            User user = await FindUsuarioById(id);  

            await _cognitoUserGroups.AddUserToGroup(user.Username, role.Role);

            return new WebApiResponse().Success();
        }

        public bool CheckRoleIsAuthorized(string role)
        {
            List<string> roles = _configuration.GetSection("AWS:Cognito:AuthorizedGroups").Get<List<string>>();

            return roles.Contains(role);
        }
    }
}
