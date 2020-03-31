using Casino.API.Data.Context;
using Casino.API.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Casino.API.Services
{
    public interface IIdentityApp
    {
        bool Authenticated { get; }

        IEnumerable<Claim> GetClaims();
        Usuario GetUser(ApplicationDbContext dbContext);
    }
}
