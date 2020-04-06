using Casino.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Casino.Data.Migrations.Configuration
{
    public class RouletteRuleTypeConfiguration : IEntityTypeConfiguration<RouletteRuleType>
    {
        public void Configure(EntityTypeBuilder<RouletteRuleType> builder)
        {
            builder.ToTable("RouletteRuleTypes");

            builder.HasData
                (
                    new { Id = 1L, Name = "Red", DefaultPay = 1f, TypeId = 1L, Numbers = (byte)18 },
                    new { Id = 2L, Name = "Black", DefaultPay = 1f, TypeId = 1L, Numbers = (byte)18 },
                    new { Id = 3L, Name = "Odd", DefaultPay = 1f, TypeId = 1L, Numbers = (byte)18 },
                    new { Id = 4L, Name = "Even", DefaultPay = 1f, TypeId = 1L, Numbers = (byte)18 },
                    new { Id = 5L, Name = "1 to 18", DefaultPay = 1f, TypeId = 1L, Numbers = (byte)18 },
                    new { Id = 6L, Name = "19 to 36", DefaultPay = 1f, TypeId = 1L, Numbers = (byte)18 },
                    new { Id = 7L, Name = "1 to 12", DefaultPay = 2f, TypeId = 1L, Numbers = (byte)12 },
                    new { Id = 8L, Name = "13 to 24", DefaultPay = 2f, TypeId = 1L, Numbers = (byte)12 },
                    new { Id = 9L, Name = "25 to 36", DefaultPay = 2f, TypeId = 1L, Numbers = (byte)12 },
                    new { Id = 10L, Name = "Six line", DefaultPay = 5f, TypeId = 1L, Numbers = (byte)6 },
                    new { Id = 11L, Name = "First five", DefaultPay = 6f, TypeId = 1L, Numbers = (byte)5 },
                    new { Id = 12L, Name = "Corner", DefaultPay = 8f, TypeId = 1L, Numbers = (byte)4 },
                    new { Id = 13L, Name = "Street", DefaultPay = 11f, TypeId = 1L, Numbers = (byte)3 },
                    new { Id = 14L, Name = "Split", DefaultPay = 17f, TypeId = 1L, Numbers = (byte)2 },
                    new { Id = 15L, Name = "One", DefaultPay = 35f, TypeId = 1L, Numbers = (byte)1 }
                );
        }
    }
}

