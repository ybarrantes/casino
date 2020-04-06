using Casino.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Casino.Data.Migrations.Configuration
{
    public class ColorConfiguration : IEntityTypeConfiguration<Color>
    {
        public void Configure(EntityTypeBuilder<Color> builder)
        {
            builder.ToTable("Colors");

            builder.HasData
                (
                    new Color { Id = 1, Name = "None" },
                    new Color { Id = 2, Name = "Red" },
                    new Color { Id = 3, Name = "Black" }
                );
        }
    }
}

