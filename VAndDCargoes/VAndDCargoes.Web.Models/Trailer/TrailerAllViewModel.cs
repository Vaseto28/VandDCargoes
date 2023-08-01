using System.ComponentModel.DataAnnotations;
using static VAndDCargoes.Common.EntitiesValidations.Trailer;

namespace VAndDCargoes.Web.ViewModels.Trailer;

public class TrailerAllViewModel
{
	[Required]
	public string Id { get; set; } = null!;

	[Range(CapacityMinValue, CapacityMaxValue)]
	public int Capacity { get; set; }

	[Required]
	public string Category { get; set; } = null!;

	[Required]
	public string Condition { get; set; } = null!;

	[Required]
	public string Dementions { get; set; } = null!;

	public string? Cargo { get; set; }
}

