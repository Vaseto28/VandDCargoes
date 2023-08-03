using System.ComponentModel.DataAnnotations;
using static VAndDCargoes.Common.EntitiesValidations.Cargo;

namespace VAndDCargoes.Web.ViewModels.Cargo;

public class CargoDetailsViewModel
{
	[Required]
	public string Id { get; set; } = null!;

	[Required]
	[StringLength(NameMaxLength, MinimumLength = NameMinLength)]
	public string Name { get; set; } = null!;

	[Required]
	[StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
	public string Description { get; set; } = null!;

	[Range(WeightMinLength, WeightMaxLength)]
	public int Weight { get; set; }

	[Required]
	public string Category { get; set; } = null!;

	[Required]
	public string PhysicalState { get; set; } = null!;

	[Required]
	public string CreatorName { get; set; } = null!;

	[Required]
	public string CreatorEmail { get; set; } = null!;

	[Required]
	public string ImageUrl { get; set; } = null!;
}

