using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAndDCargoes.Data.Models;

namespace VAndDCargoes.Data.EntitiesConfigurations;

public class DriversTrailersConfiguration : IEntityTypeConfiguration<DriversTrailers>
{
    public void Configure(EntityTypeBuilder<DriversTrailers> builder)
    {
        builder
            .HasOne(x => x.Driver)
            .WithMany(x => x.DriversTrailers)
            .HasForeignKey(x => x.DriverId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Trailer)
            .WithMany(x => x.DriversTrailers)
            .HasForeignKey(x => x.TrailerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasKey(x => new { x.DriverId, x.TrailerId });
    }
}

