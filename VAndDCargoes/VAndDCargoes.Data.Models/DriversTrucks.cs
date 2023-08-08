using System.ComponentModel.DataAnnotations;

namespace VAndDCargoes.Data.Models;

public class DriversTrucks
{
	[Required]
	public Guid DriverId { get; set; }

	public virtual Driver Driver { get; set; } = null!;

	[Required]
	public Guid TruckId { get; set; }

	public virtual Truck Truck { get; set; } = null!;
}

