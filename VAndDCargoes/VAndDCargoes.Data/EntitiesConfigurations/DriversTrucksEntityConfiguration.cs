using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAndDCargoes.Data.Models;

namespace VAndDCargoes.Data.EntitiesConfigurations;

public class DriversTrucksEntityConfiguration : IEntityTypeConfiguration<DriversTrucks>
{
    public void Configure(EntityTypeBuilder<DriversTrucks> builder)
    {
        builder
            .HasOne(x => x.Driver)
            .WithMany(x => x.DriversTrucks)
            .HasForeignKey(x => x.DriverId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Truck)
            .WithMany(x => x.DriversTrucks)
            .HasForeignKey(x => x.TruckId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasKey(x => new { x.DriverId, x.TruckId});
    }
}

