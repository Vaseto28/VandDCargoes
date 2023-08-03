using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAndDCargoes.Data.Models;

namespace VAndDCargoes.Data.EntitiesConfigurations;

public class TrailerEntityConfiguration : IEntityTypeConfiguration<Trailer>
{
    public void Configure(EntityTypeBuilder<Trailer> builder)
    {
        builder
            .HasOne(x => x.Cargo)
            .WithMany(x => x.Trailers)
            .HasForeignKey(x => x.CargoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Creator)
            .WithMany(x => x.Trailers)
            .HasForeignKey(x => x.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Property(x => x.CreatorId)
            .HasDefaultValue(Guid.Parse("3d7a05d7-8255-4936-9f32-36a07dc4af55"));

        builder
            .Property(x => x.ImageUrl)
            .HasDefaultValue("https://cpmr-islands.org/wp-content/uploads/sites/4/2019/07/test.png");
    }
}

