using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Casino.Services.DB.SQL.Contracts.Model;
using System.Threading.Tasks;

namespace Casino.API.Services
{
    public sealed class IdentityApp<T> : IIdentityApp<T> where T : class
    {
        private bool _authenticated = false;
        private bool _initialized = false;
        private IEnumerable<Claim> _claims = null;
        private T _user = null;
        private string _username = "";
        private string _cloudIdentityId = "";

        private readonly IHttpContextAccessor _httpContext;

        public IEnumerable<Claim> Claims => GetClaims();

        public IdentityApp(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;

            GetClaims();
        }

        public IEnumerable<Claim> GetClaims()
        {
            if (_claims == null)
            {
                _claims = _httpContext.HttpContext.User.Claims.ToList();

                foreach (Claim claim in Claims)
                {
                    if (claim.Type.Contains("nameidentifier"))
                    {
                        _cloudIdentityId = claim.Value;
                    }
                    else if (claim.Type.Equals("username"))
                    {
                        _username = claim.Value;
                    }
                }
            }

            return _claims;
        }

        public async Task<T> GetUser(DbContext dbContext)
        {
            if (_initialized)
                return _user;

            if (!String.IsNullOrEmpty(_username) && !String.IsNullOrEmpty(_cloudIdentityId))
            {

                _user = await dbContext.Set<T>()
                    .FirstOrDefaultAsync<T>(u => 
                        ((IEntityModelUser)u).Username.Equals(_username) &&
                        ((IEntityModelUser)u).CloudIdentityId.Equals(_cloudIdentityId));             

                _authenticated = (_user != null);
            }
            
            _initialized = true;
            
            return _user;
        }
    }
}
