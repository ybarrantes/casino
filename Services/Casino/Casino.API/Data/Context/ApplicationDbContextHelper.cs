using Microsoft.EntityFrameworkCore;
using System;

namespace Casino.API.Data.Context
{
    public class ApplicationDbContextHelper : IContext, IDisposable
    {
        private ApplicationDbContext _dbContext = null;
        private string _connectionString = null;

        public ApplicationDbContext ApplicationDbContext => _dbContext;

        public string ConnectionString
        {
            get
            {
                if (String.IsNullOrEmpty(_connectionString))
                    throw new NullReferenceException($"ConnectionString '{_connectionString}' is not valid!");

                return _connectionString;
            }
        }

        public DbContext Context
        {
            get
            {
                if(_dbContext == null)
                {
                    DbContextOptionsBuilder<ApplicationDbContext> builder = new DbContextOptionsBuilder<ApplicationDbContext>();

                    builder.UseSqlServer(ConnectionString);

                    _dbContext = new ApplicationDbContext(builder.Options);
                }

                return _dbContext;
            }
        }

        public ApplicationDbContextHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Dispose()
        {
            if (_dbContext != null)
                _dbContext.Dispose();
        }
    }
}
