using System.ComponentModel.DataAnnotations;

namespace VAndDCargoes.Data.Models;

public class DriversCargoes
{
	[Required]
	public Guid DriverId { get; set; }

	public virtual Driver Driver { get; set; } = null!;

	[Required]
	public Guid CargoId { get; set; }

	public virtual Cargo Cargo { get; set; } = null!;
}

