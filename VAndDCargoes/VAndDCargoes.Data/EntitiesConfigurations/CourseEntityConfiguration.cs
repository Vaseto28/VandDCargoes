using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAndDCargoes.Data.Models;

namespace VAndDCargoes.Data.EntitiesConfigurations;

public class CourseEntityConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder
            .HasOne(x => x.Driver)
            .WithMany(x => x.Courses)
            .HasForeignKey(x => x.DriverId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Truck)
            .WithMany(x => x.Courses)
            .HasForeignKey(x => x.TruckId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Trailer)
            .WithMany(x => x.Courses)
            .HasForeignKey(x => x.TrailerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Cargo)
            .WithMany(x => x.Courses)
            .HasForeignKey(x => x.CargoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Property(x => x.Reward)
            .HasColumnType("decimal(18,2)");
    }
}

