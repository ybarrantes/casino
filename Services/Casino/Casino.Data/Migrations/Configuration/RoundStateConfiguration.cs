using Casino.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Casino.Data.Migrations.Configuration
{
    public class RoundStateConfiguration : IEntityTypeConfiguration<RoundState>
    {
        public void Configure(EntityTypeBuilder<RoundState> builder)
        {
            builder.ToTable("RoundStates");

            builder.HasData
                (
                    new RoundState { Id = 1, State = "Opened", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                    new RoundState { Id = 2, State = "Closed", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
                );
        }
    }
}
