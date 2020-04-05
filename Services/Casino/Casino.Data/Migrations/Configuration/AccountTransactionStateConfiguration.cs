using Casino.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Casino.Data.Migrations.Configuration
{
    public class AccountTransactionStateConfiguration : IEntityTypeConfiguration<AccountTransactionState>
    {
        public void Configure(EntityTypeBuilder<AccountTransactionState> builder)
        {
            builder.ToTable("AccountTransactionStates");

            builder.HasData
                (
                    new AccountTransactionState { Id = 1, State = "Approved", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                    new AccountTransactionState { Id = 2, State = "Pending", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                    new AccountTransactionState { Id = 3, State = "Cancelled", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                    new AccountTransactionState { Id = 4, State = "Rejected", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
                );
        }
    }
}

