using AutoMapper;
using Casino.Data.Models.DTO.Rounds;
using Casino.Data.Models.DTO.Roulettes;
using Casino.Data.Models.Entities;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using Casino.Data.Models.DTO.Users;

namespace Casino.API.Config
{
    public static class ConfigureAutoMapperService
    {
        public static void AddDependencies(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(config => MapperConfigOptions(config), typeof(Startup));
        }

        public static IMapperConfigurationExpression MapperConfigOptions(IMapperConfigurationExpression config)
        {
            config.CreateMap<Roulette, RouletteCreateDTO>();
            config.CreateMap<RouletteCreateDTO, Roulette>();
            config.CreateMap<Roulette, RouletteShowDTO>();

            config.CreateMap<RouletteState, RouletteStateDTO>();
            config.CreateMap<RouletteStateDTO, RouletteState>();

            config.CreateMap<RouletteType, RouletteTypeDTO>();
            config.CreateMap<RouletteTypeDTO, RouletteType>();

            config.CreateMap<Round, RoundShowDTO>();
            config.CreateMap<RoundShowDTO, Round>();

            config.CreateMap<RoundState, RoundStateDTO>();
            config.CreateMap<RoundStateDTO, RoundState>();

            config.CreateMap<User, UserShowDTO>();

            return config;
        }
    }
}
