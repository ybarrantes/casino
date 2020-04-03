using Casino.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Casino.Data.Migrations.Configuration
{
    public class RouletteStateConfiguration : IEntityTypeConfiguration<RouletteState>
    {
        public void Configure(EntityTypeBuilder<RouletteState> builder)
        {
            builder.ToTable("RouletteStates");

            builder.HasData
                (
                    new RouletteState { Id = 1, State = "Active", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                    new RouletteState { Id = 2, State = "Inactive", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                    new RouletteState { Id = 3, State = "Suspended", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
                );
        }
    }
}
