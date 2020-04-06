using Casino.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Casino.Data.Migrations.Configuration
{
    public class NumberConfiguration : IEntityTypeConfiguration<Number>
    {
        public void Configure(EntityTypeBuilder<Number> builder)
        {
            builder.ToTable("Numbers");

            builder.HasData
                (
                    new { Id = 1L, Name = "0", ColorId = 1L },
                    new { Id = 2L, Name = "00", ColorId = 1L },
                    new { Id = 3L, Name = "000", ColorId = 1L },

                    new { Id = 4L, Name = "1", ColorId = 2L },
                    new { Id = 5L, Name = "2", ColorId = 3L },
                    new { Id = 6L, Name = "3", ColorId = 2L },
                    new { Id = 7L, Name = "4", ColorId = 3L },
                    new { Id = 8L, Name = "5", ColorId = 2L },
                    new { Id = 9L, Name = "6", ColorId = 3L },
                    new { Id = 10L, Name = "7", ColorId = 2L },
                    new { Id = 11L, Name = "8", ColorId = 3L },
                    new { Id = 12L, Name = "9", ColorId = 2L },
                    new { Id = 13L, Name = "10", ColorId = 3L },
                    new { Id = 14L, Name = "11", ColorId = 3L },
                    new { Id = 15L, Name = "12", ColorId = 2L },
                    new { Id = 16L, Name = "13", ColorId = 3L },
                    new { Id = 17L, Name = "14", ColorId = 2L },
                    new { Id = 18L, Name = "15", ColorId = 3L },
                    new { Id = 19L, Name = "16", ColorId = 2L },
                    new { Id = 20L, Name = "17", ColorId = 3L },
                    new { Id = 21L, Name = "18", ColorId = 2L },
                    new { Id = 22L, Name = "19", ColorId = 2L },
                    new { Id = 23L, Name = "20", ColorId = 3L },
                    new { Id = 24L, Name = "21", ColorId = 2L },
                    new { Id = 25L, Name = "22", ColorId = 3L },
                    new { Id = 26L, Name = "23", ColorId = 2L },
                    new { Id = 27L, Name = "24", ColorId = 3L },
                    new { Id = 28L, Name = "25", ColorId = 2L },
                    new { Id = 29L, Name = "26", ColorId = 3L },
                    new { Id = 30L, Name = "27", ColorId = 2L },
                    new { Id = 31L, Name = "28", ColorId = 3L },
                    new { Id = 32L, Name = "29", ColorId = 3L },
                    new { Id = 33L, Name = "30", ColorId = 2L },
                    new { Id = 34L, Name = "31", ColorId = 3L },
                    new { Id = 35L, Name = "32", ColorId = 2L },
                    new { Id = 36L, Name = "33", ColorId = 3L },
                    new { Id = 37L, Name = "34", ColorId = 2L },
                    new { Id = 38L, Name = "35", ColorId = 3L },
                    new { Id = 39L, Name = "36", ColorId = 2L }
                );
        }
    }
}

