using Casino.API.Components.Authentication.AwsCognito;
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

namespace Casino.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IConfiguration configuration;
        private readonly ILogger<UsersController> logger;

        public UsersController(ApplicationDbContext dbContext, IConfiguration configuration, ILogger<UsersController> logger)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
            this.logger = logger;
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
            User user = await dbContext.Users.FirstOrDefaultAsync(user => user.Id.Equals(id));

            if(user == null) throw new WebApiException(System.Net.HttpStatusCode.NotFound, $"User '{id}' not found");

            return user;
        }

        [Authorize(Policy = "SuperAdmin")]
        [HttpPost("{id}/roles")]
        public async Task<ActionResult<WebApiResponse>> AddRol(long id, [FromBody] UserRoleDTO role)
        {
            User user = await FindUsuarioById(id);

            List<string> roles = configuration.GetSection("AWS:Cognito:AuthorizedGroups").Get<List<string>>();
            if (!roles.Contains(role.Role))
                throw new WebApiException(System.Net.HttpStatusCode.BadRequest, $"The role '{role.Role}' is not authorized in aws cognito groups, see configuration");

            AwsCognitoUserGroupAuthentication userGroupAuthentication = new AwsCognitoUserGroupAuthentication(this.configuration, logger);
            await userGroupAuthentication.AddUserToGroup(user.Username, role.Role);

            return new WebApiResponse().Success();
        }
    }
}
