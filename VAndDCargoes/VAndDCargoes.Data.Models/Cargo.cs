using static VAndDCargoes.Common.EntitiesValidations.Cargo;
using System.ComponentModel.DataAnnotations;
using VAndDCargoes.Data.Models.Enumerations;

namespace VAndDCargoes.Data.Models;

public class Cargo
{
    public Cargo()
    {
        this.Id = Guid.NewGuid();
        this.Trailers = new HashSet<Trailer>();
    }

    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(NameMaxLength)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(DescriptionMaxLength)]
    public string Description { get; set; } = null!;

    public int Weight { get; set; }

    [Required]
    public CargoCategory Category { get; set; }

    [Required]
    public CargoPhysicalState PhysicalState { get; set; }

    public virtual ICollection<Trailer> Trailers { get; set; }
}

