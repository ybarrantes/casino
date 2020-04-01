using Casino.Services.DB.SQL.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;

namespace Casino.API.Services
{
    public interface IIdentityApp<T> where T : class
    {
        IEnumerable<Claim> GetClaims();
        T GetUser(DbContext dbContext);
    }
}
