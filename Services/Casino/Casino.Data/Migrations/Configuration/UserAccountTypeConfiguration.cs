using Casino.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Casino.Data.Migrations.Configuration
{
    public class UserAccountTypeConfiguration : IEntityTypeConfiguration<UserAccountType>
    {
        public void Configure(EntityTypeBuilder<UserAccountType> builder)
        {
            builder.ToTable("UserAccountTypes");

            builder.HasData
                (
                    new UserAccountType { Id = 1, Type = "Free", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                    new UserAccountType { Id = 2, Type = "Real", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
                );
        }
    }
}
