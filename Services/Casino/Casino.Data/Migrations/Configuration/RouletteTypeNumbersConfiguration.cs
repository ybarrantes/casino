using Casino.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Casino.Data.Migrations.Configuration
{
    public class RouletteTypeNumbersConfiguration : IEntityTypeConfiguration<RouletteTypeNumber>
    {
        public void Configure(EntityTypeBuilder<RouletteTypeNumber> builder)
        {
            builder.ToTable("RouletteTypeNumbers");

            builder.HasData
                (
                    new { Id = 1L, TypeId = 1L, NumberId = 1L },
                    new { Id = 2L, TypeId = 1L, NumberId = 4L },
                    new { Id = 3L, TypeId = 1L, NumberId = 5L },
                    new { Id = 4L, TypeId = 1L, NumberId = 6L },
                    new { Id = 5L, TypeId = 1L, NumberId = 7L },
                    new { Id = 6L, TypeId = 1L, NumberId = 8L },
                    new { Id = 7L, TypeId = 1L, NumberId = 9L },
                    new { Id = 8L, TypeId = 1L, NumberId = 10L },
                    new { Id = 9L, TypeId = 1L, NumberId = 11L },
                    new { Id = 10L, TypeId = 1L, NumberId = 12L },
                    new { Id = 11L, TypeId = 1L, NumberId = 13L },
                    new { Id = 12L, TypeId = 1L, NumberId = 14L },
                    new { Id = 13L, TypeId = 1L, NumberId = 15L },
                    new { Id = 14L, TypeId = 1L, NumberId = 16L },
                    new { Id = 15L, TypeId = 1L, NumberId = 17L },
                    new { Id = 16L, TypeId = 1L, NumberId = 18L },
                    new { Id = 17L, TypeId = 1L, NumberId = 19L },
                    new { Id = 18L, TypeId = 1L, NumberId = 20L },
                    new { Id = 19L, TypeId = 1L, NumberId = 21L },
                    new { Id = 20L, TypeId = 1L, NumberId = 22L },
                    new { Id = 21L, TypeId = 1L, NumberId = 23L },
                    new { Id = 22L, TypeId = 1L, NumberId = 24L },
                    new { Id = 23L, TypeId = 1L, NumberId = 25L },
                    new { Id = 24L, TypeId = 1L, NumberId = 26L },
                    new { Id = 25L, TypeId = 1L, NumberId = 27L },
                    new { Id = 26L, TypeId = 1L, NumberId = 28L },
                    new { Id = 27L, TypeId = 1L, NumberId = 29L },
                    new { Id = 28L, TypeId = 1L, NumberId = 30L },
                    new { Id = 29L, TypeId = 1L, NumberId = 31L },
                    new { Id = 30L, TypeId = 1L, NumberId = 32L },
                    new { Id = 31L, TypeId = 1L, NumberId = 33L },
                    new { Id = 32L, TypeId = 1L, NumberId = 34L },
                    new { Id = 33L, TypeId = 1L, NumberId = 35L },
                    new { Id = 34L, TypeId = 1L, NumberId = 36L },
                    new { Id = 35L, TypeId = 1L, NumberId = 37L },
                    new { Id = 36L, TypeId = 1L, NumberId = 38L },
                    new { Id = 37L, TypeId = 1L, NumberId = 39L }
                );
        }
    }
}


