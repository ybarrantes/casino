using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Casino.API.Services
{
    public interface IIdentityApp<T> where T : class
    {
        IEnumerable<Claim> GetClaims();
        Task<T> GetUser(DbContext dbContext);
    }
}
