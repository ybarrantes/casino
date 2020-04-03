using Casino.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Casino.UnitTests
{
    class Helpers
    {
        private static IConfigurationRoot config = null;
        private static ApplicationDbContext dbContext = null;

        public static IConfigurationRoot GetConfiguration()
        {
            if(config == null)
            {
                config = new ConfigurationBuilder()
                    //.SetBasePath(outputPath)
                    .AddJsonFile("appsettings.json", optional: true)
                    .AddJsonFile("appsettings.Development.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();
            }

            return config;
        }

        public static DbContextOptions<ApplicationDbContext> OptionsDBContext(string dbName)
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
        }

        public static ApplicationDbContext GetDbContext()
        {
            if(dbContext == null)
            {
                dbContext = GetNewDbContext();
            }

            return dbContext;
        }

        public static ApplicationDbContext GetNewDbContext(string dbName = "CasinoUnitTestDb")
        {
            return new ApplicationDbContext(OptionsDBContext(dbName));
        }
    }
}
