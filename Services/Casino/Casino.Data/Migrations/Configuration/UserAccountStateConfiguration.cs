using Casino.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Casino.Data.Migrations.Configuration
{
    public class UserAccountStateConfiguration : IEntityTypeConfiguration<UserAccountState>
    {
        public void Configure(EntityTypeBuilder<UserAccountState> builder)
        {
            builder.ToTable("UserAccountStates");

            builder.HasData
                (
                    new UserAccountState { Id = 1, State = "Active", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                    new UserAccountState { Id = 2, State = "Inactive", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                    new UserAccountState { Id = 3, State = "Suspended", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
                );
        }
    }
}
