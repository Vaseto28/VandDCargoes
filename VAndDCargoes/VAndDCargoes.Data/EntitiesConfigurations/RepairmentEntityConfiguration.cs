using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAndDCargoes.Data.Models;
using VAndDCargoes.Data.Models.Enumerations;

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

        builder
            .Property(x => x.Quantity)
            .HasDefaultValue(1);

        builder
            .Property(x => x.Type)
            .HasDefaultValue((RepairmentAvailableTypes)0);
    }
}

