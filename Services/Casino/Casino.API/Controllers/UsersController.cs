using Casino.API.Components.Authentication.AwsCognito;
using Casino.API.Data.Context;
using Casino.API.Data.Entities;
using Casino.API.Data.Extension;
using Casino.API.Data.Models.User;
using Casino.API.Exceptions;
using Casino.API.Util.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<ActionResult<HttpResponse>> GetUser(long id)
        {
            User user = await FindUsuarioById(id);
            return new HttpResponse().Success().SetData(user);
        }

        private async Task<User> FindUsuarioById(long id)
        {
            User user = await dbContext.Users.FirstOrDefaultAsync(user => user.Id.Equals(id));

            if(user == null) throw new HttpResponseException(System.Net.HttpStatusCode.NotFound, $"User '{id}' not found");

            return user;
        }

        [Authorize(Policy = "SuperAdmin")]
        [HttpPost("{id}/roles")]
        public async Task<ActionResult<HttpResponse>> AddRol(long id, [FromBody] RoleDTO role)
        {
            User user = await FindUsuarioById(id);

            List<string> roles = configuration.GetSection("AWS:Cognito:AuthorizedGroups").Get<List<string>>();
            if (!roles.Contains(role.Role))
                throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest, $"The role '{role.Role}' is not authorized in aws cognito groups, see configuration");

            AwsCognitoUserGroupAuthentication userGroupAuthentication = new AwsCognitoUserGroupAuthentication(this.configuration, logger);
            await userGroupAuthentication.AddUserToGroup(user.Username, role.Role);

            return new HttpResponse().Success();
        }
    }
}
