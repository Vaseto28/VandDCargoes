using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAndDCargoes.Data.Models;

namespace VAndDCargoes.Data.EntitiesConfigurations;

public class DriverEntityConfiguration : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder
            .Property(x => x.Ballance)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0m);
    }
}

