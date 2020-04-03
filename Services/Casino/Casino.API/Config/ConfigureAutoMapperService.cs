using AutoMapper;
using Casino.Data.Models.DTO.Rounds;
using Casino.Data.Models.DTO.Roulettes;
using Casino.Data.Models.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Casino.API.Config
{
    static class ConfigureAutoMapperService
    {
        public static void AddDependencies(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(config =>
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
            }, typeof(Startup));
        }
    }
}
