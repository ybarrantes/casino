using Casino.API.Components.Authentication.AwsCognito;
using Casino.API.Data.Context;
using Casino.API.Data.Entities;
using Casino.API.Data.Extension;
using Casino.API.Data.Models.Usuario;
using Casino.API.Exceptions;
using Casino.API.Util.Logging;
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
    [Route("api/usuarios")]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IConfiguration configuration;
        private readonly ILogger<UsuariosController> logger;

        public UsuariosController(ApplicationDbContext dbContext, IConfiguration configuration, ILogger<UsuariosController> logger)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
            this.logger = logger;
        }

        [Authorize]
        [HttpGet("{id}", Name = "GetUsuario")]
        public async Task<ActionResult<HttpResponse>> GetUsuario(long id)
        {
            Usuario usuario = await FindUsuarioById(id);
            return new HttpResponse().Success().SetData(usuario);
        }

        private async Task<Usuario> FindUsuarioById(long id)
        {
            Usuario usuario = await dbContext.Usuarios.FirstOrDefaultAsync(user => user.Id.Equals(id));

            if(usuario == null) throw new HttpResponseException(System.Net.HttpStatusCode.NotFound, $"User '{id}' not found");

            return usuario;
        }

        [Authorize(Policy = "SuperAdmin")]
        [HttpPost("{id}/roles")]
        public async Task<ActionResult<HttpResponse>> AddRol(long id, [FromBody] RolDTO rol)
        {
            Usuario usuario = await FindUsuarioById(id);

            List<string> roles = configuration.GetSection("AWS:Cognito:AuthorizedGroups").Get<List<string>>();
            if (!roles.Contains(rol.Rol))
                throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest, $"The rol '{rol.Rol}' is not authorized in aws cognito groups, see configuration");

            AwsCognitoUserGroupAuthentication userGroupAuthentication = new AwsCognitoUserGroupAuthentication(this.configuration, logger);
            await userGroupAuthentication.AddUserToGroup(usuario.Username, rol.Rol);

            return new HttpResponse().Success();
        }
    }
}
