using Casino.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Casino.Data.Migrations.Configuration
{
    public class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
    {
        public void Configure(EntityTypeBuilder<UserAccount> builder)
        {
            builder.ToTable("UserAccounts");

            builder.HasData
                (
                    new
                    {
                        Id = 1L,
                        StateId = 1L,
                        TypeId = 1L,
                        UserOwnerId = 1L                        
                    },
                    new
                    {
                        Id = 2L,
                        StateId = 1L,
                        TypeId = 2L,
                        UserOwnerId = 1L
                    }
                );
        }
    }
}


