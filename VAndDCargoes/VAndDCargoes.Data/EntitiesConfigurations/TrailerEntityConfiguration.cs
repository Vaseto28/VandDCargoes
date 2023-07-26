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
    }
}

