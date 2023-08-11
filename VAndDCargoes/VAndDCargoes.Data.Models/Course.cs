using System.ComponentModel.DataAnnotations;
using static VAndDCargoes.Common.EntitiesValidations.Course;

namespace VAndDCargoes.Data.Models;

public class Course
{
	public Course()
	{
		this.Id = Guid.NewGuid();
	}

	[Key]
	public Guid Id { get; set; }

	[Required]
	[MaxLength(DepartureCityMaxLength)]
	public string DepartureCity { get; set; } = null!;

	[Required]
	[MaxLength(ArrivalCityMaxLength)]
	public string ArrivalCity { get; set; } = null!;

	public int Distance { get; set; }

	public decimal Reward { get; set; }

	[Required]
	public Guid DriverId { get; set; }

	public virtual Driver Driver { get; set; } = null!;

	[Required]
	public Guid TruckId { get; set; }

	public virtual Truck Truck { get; set; } = null!;

	[Required]
	public Guid TrailerId { get; set; }

	public virtual Trailer Trailer { get; set; } = null!;

	[Required]
	public Guid CargoId { get; set; }

	public virtual Cargo Cargo { get; set; } = null!;
}

