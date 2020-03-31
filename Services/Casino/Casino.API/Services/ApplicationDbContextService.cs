using Casino.API.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Services
{
    public class ApplicationDbContextService : IApplicationDbContextService, IDisposable
    {
        private ApplicationDbContext _dbContext = null;

        public ApplicationDbContext Context => _dbContext;

        public ApplicationDbContextService(IConfiguration configuration)
        {
            DbContextOptionsBuilder<ApplicationDbContext> builder = new DbContextOptionsBuilder<ApplicationDbContext>();

            string connection = configuration.GetConnectionString("DefaultConnection");

            builder.UseSqlServer(connection);

            _dbContext = new ApplicationDbContext(builder.Options);
        }

        public void Dispose()
        {
            if(_dbContext != null)
                _dbContext.Dispose();
        }
    }
}
