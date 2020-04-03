using Casino.API.Components;
using Casino.API.Components.Roulettes;
using Casino.API.Components.Rounds;
using Casino.API.Services;
using Casino.Data.Models.Entities;
using Casino.Services.Authentication;
using Casino.Services.Authentication.AwsCognito;
using Casino.Services.Authentication.Contracts;
using Casino.Services.DB.SQL;
using Casino.Services.DB.SQL.Contracts.CRUD;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Casino.API.Config
{
    static class ConfigureCustomServices
    {
        public static void AddDependencies(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IAuthentication), typeof(AwsCognitoAuthentication));
            services.AddScoped(typeof(IAwsCognitoUserGroups), typeof(AwsCognitoUserGroups));
            services.AddScoped(typeof(IIdentityApp<>), typeof(IdentityApp<>));
            services.AddScoped(typeof(ContextCRUD<>), typeof(ContextSqlCRUD<>));
            services.AddScoped<CRUDComponent<Roulette>, RoulettesCRUDComponent>();
            services.AddScoped<CRUDComponent<RouletteState>, RoulettesStatesCRUDComponent>();
            services.AddScoped<CRUDComponent<RouletteType>, RoulettesTypesCRUDComponent>();
            services.AddScoped<CRUDComponent<Round>, RoundsCRUDComponent>();
        }
    }
}
