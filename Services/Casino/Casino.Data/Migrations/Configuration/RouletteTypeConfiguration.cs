using Casino.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Casino.Data.Migrations.Configuration
{
    public class RouletteTypeConfiguration : IEntityTypeConfiguration<RouletteType>
    {
        public void Configure(EntityTypeBuilder<RouletteType> builder)
        {
            builder.ToTable("RouletteTypes");

            builder.HasData
                (
                    new RouletteType { Id = 1, Type = "European", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                    new RouletteType { Id = 2, Type = "American", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
                );
        }
    }
}
