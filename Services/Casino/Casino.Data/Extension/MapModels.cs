using AutoMapper;
using Casino.Data.Models.DTO;
using Casino.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Casino.Data.Extension
{
    public static class MapModels
    {
        public static IMapperConfigurationExpression GetMapper(IMapperConfigurationExpression config)
        {
            config.CreateMap<Roulette, RouletteCreateDTO>();
            config.CreateMap<RouletteCreateDTO, Roulette>();
            config.CreateMap<Roulette, RouletteShowDTO>();

            config.CreateMap<Domain, DomainShowDTO>();
            config.CreateMap<DomainShowDTO, Domain>();

            return config;
        }
    }
}
