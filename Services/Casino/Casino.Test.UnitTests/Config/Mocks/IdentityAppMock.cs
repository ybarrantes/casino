using Casino.API.Services;
using Casino.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Test.UnitTests.Config.Mocks
{
    public class IdentityAppMock : IIdentityApp<User>
    {
        public IEnumerable<Claim> GetClaims()
        {
            return null;
        }

        public async Task<User> GetUser(DbContext dbContext)
        {
            return await Task.Run(() => (new User { CloudIdentityId = "xxxxxxx", Username = "test", Email = "test@test.com", Id = 1 }));
        }
    }
}
