using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using Casino.Data.Models.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using Casino.Services.DB.SQL.Contracts;
using Casino.Services.DB.SQL.Contracts.Model;
using Casino.Data.Migrations.Configuration;
using Casino.Data.Models.Views;
using Casino.Services.DB.SQL.Context;

namespace Casino.Data.Context
{
    public class ApplicationDbContext : ApplicationDbContextBase
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
        }
        

        #region Datasets

        public DbSet<User> Users { get; set; }

        public DbSet<RouletteType> RouletteTypes { get; set; }
        public DbSet<RouletteState> RouletteStates { get; set; }
        public DbSet<Roulette> Roulettes { get; set; }

        public DbSet<RouletteRuleType> RouletteRuleTypes { get; set; }
        public DbSet<RouletteRule> RouletteRules { get; set; }

        public DbSet<RoundState> RoundStates { get; set; }
        public DbSet<Round> Rounds { get; set; }

        public DbSet<UserAccountType> UserAccountTypes { get; set; }
        public DbSet<UserAccountState> UserAccountStates { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }

        public DbSet<Bet> Bets { get; set; }

        public DbSet<AccountTransactionState> AccountTransactionStates { get; set; }
        public DbSet<AccountTransactionType> AccountTransactionTypes { get; set; }
        public DbSet<AccountTransaction> AccountTransactions { get; set; }

        // this is a view, not real table
        public DbSet<UserAccountBalance> UserAccountBalances { get; set; }

        #endregion


        // TODO: add filter to skip records marked as soft-deleted
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Bet>();
            modelBuilder.Ignore<UserAccountBalance>();

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RouletteStateConfiguration());
            modelBuilder.ApplyConfiguration(new RouletteTypeConfiguration());

            modelBuilder.ApplyConfiguration(new RoundStateConfiguration());
            
            modelBuilder.ApplyConfiguration(new UserAccountStateConfiguration());
            modelBuilder.ApplyConfiguration(new UserAccountTypeConfiguration());

            modelBuilder.ApplyConfiguration(new AccountTransactionStateConfiguration());
            modelBuilder.ApplyConfiguration(new AccountTransactionTypeConfiguration());

            modelBuilder.ApplyConfiguration(new RouletteRuleTypeConfiguration());
        }
    }
}
