using System.ComponentModel.DataAnnotations;
using static VAndDCargoes.Common.EntitiesValidations.Repairment;

namespace VAndDCargoes.Data.Models;

public class Repairment
{
	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(TypeMaxLength)]
	public string Type { get; set; } = null!;

	[Required]
	[MaxLength(DescriptionMaxLenght)]
	public string Description { get; set; } = null!;

	public int Cost { get; set; }

	[Required]
	public string MadeOn { get; set; } = null!;

	public Guid MechanicId { get; set; }

	public virtual ApplicationUser Mechanic { get; set; } = null!;
}

