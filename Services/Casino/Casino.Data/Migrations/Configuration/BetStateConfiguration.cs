using Casino.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Casino.Data.Migrations.Configuration
{
    public class BetStateConfiguration : IEntityTypeConfiguration<BetState>
    {
        public void Configure(EntityTypeBuilder<BetState> builder)
        {
            builder.ToTable("BetStates");

            builder.HasData
                (
                    new BetState { Id = 1, State = "Active", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                    new BetState { Id = 2, State = "Canceled", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
                );
        }
    }
}

