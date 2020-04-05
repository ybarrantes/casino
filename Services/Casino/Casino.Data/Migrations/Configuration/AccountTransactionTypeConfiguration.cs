using Casino.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Casino.Data.Migrations.Configuration
{
    public class AccountTransactionTypeConfiguration : IEntityTypeConfiguration<AccountTransactionType>
    {
        public void Configure(EntityTypeBuilder<AccountTransactionType> builder)
        {
            builder.ToTable("AccountTransactionTypes");

            builder.HasData
                (
                    new AccountTransactionType { Id = 1, Type = "Deposit", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                    new AccountTransactionType { Id = 2, Type = "Withdrawal", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                    new AccountTransactionType { Id = 3, Type = "Bet", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                    new AccountTransactionType { Id = 4, Type = "Bonus", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
                );
        }
    }
}
