using System.ComponentModel.DataAnnotations;

namespace VAndDCargoes.Data.Models;

public class DriversTrailers
{
	[Required]
	public Guid DriverId { get; set; }

	public virtual Driver Driver { get; set; } = null!;

	[Required]
	public Guid TrailerId { get; set; }

	public virtual Trailer Trailer { get; set; } = null!;
}

