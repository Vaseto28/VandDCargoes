using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAndDCargoes.Data.Models;

namespace VAndDCargoes.Data.EntitiesConfigurations;

public class RepairmentEntityConfiguration : IEntityTypeConfiguration<Repairment>
{
    public void Configure(EntityTypeBuilder<Repairment> builder)
    {
        builder
            .HasOne(x => x.Mechanic)
            .WithMany(x => x.Repairments)
            .HasForeignKey(x => x.MechanicId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

