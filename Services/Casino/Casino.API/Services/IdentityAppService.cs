using Casino.API.Data.Context;
using Casino.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Casino.API.Services
{
    public class IdentityAppService : IIdentityApp
    {
        private bool _authenticated = false;
        private bool _initialized = false;
        private IEnumerable<Claim> _claims = null;
        private Usuario _user = null;
        private string _username = "";
        private string _cloudIdentityId = "";

        private readonly IHttpContextAccessor _httpContext;

        public bool Authenticated => _authenticated;
        public IEnumerable<Claim> Claims => GetClaims();

        bool IIdentityApp.Authenticated => throw new NotImplementedException();

        public IdentityAppService(IHttpContextAccessor httpContext)
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

        public Usuario GetUser(ApplicationDbContext dbContext)
        {
            if (_initialized)
                return _user;

            if (!String.IsNullOrEmpty(_username) && !String.IsNullOrEmpty(_cloudIdentityId))
            {

                _user = dbContext.Usuarios
                    .FirstOrDefault<Usuario>(u =>
                    u.Username.Equals(_username) && u.CloudIdentityId.Equals(_cloudIdentityId));
             

                _authenticated = (_user != null);
            }
            
            _initialized = true;
            
            return _user;
        }
    }
}
