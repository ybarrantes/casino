using AutoMapper;
using Casino.Data.Models.DTO.Rounds;
using Casino.Data.Models.DTO.Roulettes;
using Casino.Data.Models.Entities;
using Microsoft.Extensions.DependencyInjection;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using Casino.Data.Models.DTO.Users;
using Casino.Data.Models.DTO.UserAccounts;
using Casino.Data.Models.Views;
using Casino.Data.Models.DTO.AccountTransanctions;
using Casino.Data.Models.DTO.Bets;

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

            config.CreateMap<Round, RoundShowDTO>();

            config.CreateMap<RoundState, RoundStateDTO>();

            config.CreateMap<User, UserShowDTO>();

            config.CreateMap<UserAccount, UserAccountShowDTO>();
            config.CreateMap<UserAccountState, UserAccountStateShowDTO>();
            config.CreateMap<UserAccountType, UserAccountTypeShowDTO>();

            config.CreateMap<UserAccountWithBalanceDTO, UserAccountShowDTO>();
            config.CreateMap<UserAccountBalance, UserAccountShowDTO>();

            config.CreateMap<AccountTransaction, AccountTransactionShowDTO>();
            config.CreateMap<AccountTransactionState, AccountTransactionStateShowDTO>();
            config.CreateMap<AccountTransactionType, AccountTransactionTypeShowDTO>();

            config.CreateMap<Round, RoundForBetsShowDTO>();

            config.CreateMap<RouletteRuleType, RouletteRuleTypeShowDTO>();
            config.CreateMap<RouletteRule, RouletteRulesShowDTO>();
            config.CreateMap<Roulette, RouletteBetsShowDTO>();
            config.CreateMap<Round, RoundForBetsShowDTO>();

            config.CreateMap<Bet, BetShowDTO>();
            config.CreateMap<BetState, BetStateShowDTO>();

            return config;
        }
    }
}
