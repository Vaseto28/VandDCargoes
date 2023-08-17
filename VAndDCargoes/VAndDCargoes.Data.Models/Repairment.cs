using System.ComponentModel.DataAnnotations;
using VAndDCargoes.Data.Models.Enumerations;
using static VAndDCargoes.Common.EntitiesValidations.Repairment;

namespace VAndDCargoes.Data.Models;

public class Repairment
{
	[Key]
	public int Id { get; set; }

	[Required]
	public RepairmentAvailableTypes Type { get; set; }

	[Required]
	[MaxLength(DescriptionMaxLenght)]
	public string Description { get; set; } = null!;

	public int Quantity { get; set; }

	public int Cost { get; set; }

	[Required]
	public string MadeOn { get; set; } = null!;

	public Guid MechanicId { get; set; }

	public virtual ApplicationUser Mechanic { get; set; } = null!;
}

