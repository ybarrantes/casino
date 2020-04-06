using Casino.API.Components;
using Casino.API.Components.AccountTransactions;
using Casino.API.Components.Roulettes;
using Casino.API.Components.Rounds;
using Casino.API.Components.UserAccounts;
using Casino.API.Components.Users;
using Casino.API.Services;
using Casino.Data.Models.Entities;
using Casino.Services.Authentication;
using Casino.Services.Authentication.AwsCognito;
using Casino.Services.Authentication.Contracts;
using Casino.Services.DB.SQL.Crud;
using Casino.Services.Util.Collections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Casino.API.Config
{
    public static class ConfigureCustomServices
    {
        public static void AddDependencies(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IAuthentication), typeof(AwsCognitoAuthentication));

            services.AddScoped(typeof(IAwsCognitoUserGroups), typeof(AwsCognitoUserGroups));

            services.AddScoped(typeof(IIdentityApp<>), typeof(IdentityAppMock<>));

            services.AddScoped(typeof(IPagedRecords<>), typeof(PagedRecords<>));

            services.AddScoped(typeof(ISqlContextCrud<>), typeof(SqlContextCrud<>));

            services.AddScoped<ISqlContextCrud<User>, UsersCrudComponent>();
            services.AddScoped<ISqlContextCrud<Roulette>, RoulettesCrudComponent>();
            services.AddScoped<ISqlContextCrud<RouletteState>, RoulettesStatesCrudComponent>();
            services.AddScoped<ISqlContextCrud<RouletteType>, RoulettesTypesCrudComponent>();
            services.AddScoped<ISqlContextCrud<Round>, RoundsCrudComponent>();
            services.AddScoped<ISqlContextCrud<UserAccount>, UserAccountsCrudComponent>();
            services.AddScoped<ISqlContextCrud<AccountTransaction>, AccountTransactionsCrudComponent>();
        }
    }
}
