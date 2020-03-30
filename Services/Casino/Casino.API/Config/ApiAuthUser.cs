using Casino.API.Data.Context;
using Casino.API.Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Casino.API.Config
{
    public class ApiAuthUser
    {
        private static ApiAuthUser instance = null;

        public bool Authenticated { get; }
        public IEnumerable<Claim> Claims { get; } = null;
        public Usuario User { get; } = null;

        public ApiAuthUser(IHttpContextAccessor httpContext, ApplicationDbContext dbContext)
        {
            try
            {
                Authenticated = false;

                Claims = httpContext.HttpContext.User.Claims.ToList();
                string cloudIdentityId = "";
                string username = "";

                foreach (Claim claim in Claims)
                {
                    if (claim.Type.Contains("nameidentifier"))
                    {
                        cloudIdentityId = claim.Value;
                    }
                    else if (claim.Type == "username")
                    {
                        username = claim.Value;
                    }
                }

                if(!String.IsNullOrEmpty(cloudIdentityId) && !String.IsNullOrEmpty(username))
                {
                    User = dbContext.Usuarios
                        .Where<Usuario>(u => u.Username.Equals(username) && u.CloudIdentityId.Equals(cloudIdentityId))
                        .FirstOrDefault();

                    Authenticated = (User != null);
                }
            }
            catch (Exception)
            {
            }
        }

        public static ApiAuthUser Singleton(IHttpContextAccessor httpContext, ApplicationDbContext dbContext, bool reInstance = false)
        {
            if(reInstance || instance == null)
            {
                instance = new ApiAuthUser(httpContext, dbContext);
            }

            return instance;
        }
    }
}
