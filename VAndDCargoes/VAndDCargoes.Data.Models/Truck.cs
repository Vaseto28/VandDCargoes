using System.ComponentModel.DataAnnotations;
using VAndDCargoes.Data.Models.Enumerations;
using static VAndDCargoes.Common.EntitiesValidations.Truck;

namespace VAndDCargoes.Data.Models;

public class Truck
{
    public Truck()
    {
        this.Id = Guid.NewGuid();

        this.DriversTrucks = new HashSet<DriversTrucks>();
        this.Courses = new HashSet<Course>();
    }

    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(MakeMaxLength)]
    public string Make { get; set; } = null!;

    [Required]
    [MaxLength(ModelMaxLength)]
    public string Model { get; set; } = null!;

    [Required]
    [MaxLength(RegistrationNumberMaxLength)]
    public string RegistrationNumber { get; set; } = null!;

    [Required]
    public string ImageUrl { get; set; } = null!;

    [Required]
    public TruckCondition Condition { get; set; }

    public int TraveledDistance { get; set; }

    public int FuelCapacity { get; set; }

    [Required]
    public DateTime CreatedOn { get; set; }

    [Required]
    public Guid CreatorId { get; set; }

    public virtual ApplicationUser Creator { get; set; } = null!;

    public virtual ICollection<DriversTrucks> DriversTrucks { get; set; } = null!;

    public virtual ICollection<Course> Courses { get; set; } = null!;
}

