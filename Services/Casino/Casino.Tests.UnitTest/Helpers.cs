using AutoMapper;
using AutoMapper.Configuration;
using Casino.Data.Context;
using Casino.Data.Models.DTO.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Casino.UnitTests
{
    public class Helpers
    {
        private static IConfigurationRoot _configRoot = null;
        private static ApplicationDbContext _dbContext = null;
        private static IMapper _mapper = null;

        public static IConfigurationRoot GetConfiguration()
        {
            if (_configRoot == null)
            {
                _configRoot = new ConfigurationBuilder()
                    //.SetBasePath(outputPath)
                    .AddJsonFile("appsettings.json", optional: true)
                    .AddJsonFile("appsettings.Development.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();
            }

            return _configRoot;
        }

        public static IMapper GetAutoMapperConfiguration()
        {
            if(_mapper == null)
            {
                MapperConfigurationExpression configExpression = new MapperConfigurationExpression();
                configExpression = (MapperConfigurationExpression)API.Config.ConfigureAutoMapperService.MapperConfigOptions(configExpression);

                var mapperConfig = new MapperConfiguration(configExpression);

                _mapper = new Mapper(mapperConfig);
            }

            return _mapper;
        }

        public static DbContextOptions<ApplicationDbContext> OptionsDBContext(string dbName)
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
        }

        public static ApplicationDbContext GetDbContext()
        {
            if (_dbContext == null)
            {
                _dbContext = GetNewDbContext();
            }

            return _dbContext;
        }

        public static ApplicationDbContext GetNewDbContext(string dbName = "CasinoUnitTestDb")
        {
            return new ApplicationDbContext(OptionsDBContext(dbName));
        }

        public static UserSignInDTO GetDefaultUsernameAndPassword()
        {
            return new UserSignInDTO
            {
                Username = "yonicristhbl",
                Password = "123456Yb"
            };
        }
    }
}
