using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAndDCargoes.Data.Models;

namespace VAndDCargoes.Data.EntitiesConfigurations;

public class TruckEntityConfiguration : IEntityTypeConfiguration<Truck>
{
    public void Configure(EntityTypeBuilder<Truck> builder)
    {
        builder
            .HasOne(x => x.Driver)
            .WithMany(x => x.Trucks)
            .HasForeignKey(x => x.DriverId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Trailer)
            .WithMany(x => x.Trucks)
            .HasForeignKey(x => x.TraillerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Creator)
            .WithMany(x => x.Trucks)
            .HasForeignKey(x => x.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

