using System.ComponentModel.DataAnnotations;
using static VAndDCargoes.Common.EntitiesValidations.Cargo;

namespace VAndDCargoes.Web.ViewModels.Cargo;

public class CargoEditViewModel
{
	[Required]
	[StringLength(NameMaxLength, MinimumLength = NameMinLength)]
	public string Name { get; set; } = null!;

	[Required]
	[StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
	public string Description { get; set; } = null!;

	[Range(WeightMinLength, WeightMaxLength)]
	public int Weight { get; set; }

	public int Category { get; set; }

	public int PhysicalState { get; set; }

	[Required]
	[Url]
	[Display(Name = "Image url / (link)")]
	public string ImageUrl { get; set; } = null!;
}

