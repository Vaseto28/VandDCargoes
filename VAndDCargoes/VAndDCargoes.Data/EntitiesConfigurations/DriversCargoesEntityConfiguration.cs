using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAndDCargoes.Data.Models;

namespace VAndDCargoes.Data.EntitiesConfigurations;

public class DriversCargoesEntityConfiguration : IEntityTypeConfiguration<DriversCargoes>
{
    public void Configure(EntityTypeBuilder<DriversCargoes> builder)
    {
        builder
            .HasOne(x => x.Driver)
            .WithMany(x => x.DriversCargoes)
            .HasForeignKey(x => x.DriverId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Cargo)
            .WithMany(x => x.DriversCargoes)
            .HasForeignKey(x => x.CargoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasKey(x => new { x.DriverId, x.CargoId });
    }
}

