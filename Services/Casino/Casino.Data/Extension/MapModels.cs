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
            

            return config;
        }
    }
}
