using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAndDCargoes.Data.Models;

namespace VAndDCargoes.Data.EntitiesConfigurations;

public class TruckEntityConfiguration : IEntityTypeConfiguration<Truck>
{
    public void Configure(EntityTypeBuilder<Truck> builder)
    {
        builder
            .HasOne(x => x.Creator)
            .WithMany(x => x.Trucks)
            .HasForeignKey(x => x.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Property(x => x.CreatedOn)
            .HasDefaultValue(DateTime.Parse("01/01/2023"));

        builder
            .Property(x => x.ImageUrl)
            .HasDefaultValue("https://cpmr-islands.org/wp-content/uploads/sites/4/2019/07/test.png");
    }
}

