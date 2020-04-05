using Casino.Services.DB.SQL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Casino.Data.Context
{
    public class ApplicationDbContextHelper : IContext, IDisposable
    {
        private ApplicationDbContext _dbContext = null;
        private string _connectionString = null;
        private readonly ILogger<ApplicationDbContext> _logger = null;

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

        public ApplicationDbContextHelper(ILogger<ApplicationDbContext> logger, string connectionString)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public void Dispose()
        {
            if (_dbContext != null)
                _dbContext.Dispose();
        }
    }
}
