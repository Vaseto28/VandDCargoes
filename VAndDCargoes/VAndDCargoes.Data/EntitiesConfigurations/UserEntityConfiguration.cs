using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAndDCargoes.Data.Models;

namespace VAndDCargoes.Data.EntitiesConfigurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder
            .Property(x => x.Username)
            .HasDefaultValue("Test123");
    }
}

