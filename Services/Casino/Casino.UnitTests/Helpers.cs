using Casino.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Casino.UnitTests
{
    class Helpers
    {
        private static IConfigurationRoot config = null;
        private static DbContextOptions<ApplicationDbContext> optionsDbContext = null;
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

        public static DbContextOptions<ApplicationDbContext> OptionsDBContext()
        {
            if(optionsDbContext == null)
            {
                optionsDbContext = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "CasinoUnitTestDb")
                    .Options;
            }

            return optionsDbContext;
        }

        public static ApplicationDbContext GetDbContext()
        {
            if(dbContext == null)
            {
                dbContext = GetNewDbContext();
            }

            return dbContext;
        }

        public static ApplicationDbContext GetNewDbContext()
        {
            ILogger<ApplicationDbContext> logger = Moq.Mock.Of<ILogger<ApplicationDbContext>>();

            return new ApplicationDbContext(OptionsDBContext());
        }
    }
}
