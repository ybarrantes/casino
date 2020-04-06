using Casino.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Casino.Data.Migrations.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasData
                (
                    new User { 
                        Id = 1, 
                        Username = "system-1", 
                        Email = "ybarrantes.juntos085@gmail.com", 
                        CloudIdentityId = "1ca11719-2566-4da7-87f3-fc65ddc591ad" 
                    }
                );
        }
    }
}


