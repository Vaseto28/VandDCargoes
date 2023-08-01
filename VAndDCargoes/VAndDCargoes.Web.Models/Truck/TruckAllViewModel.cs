using System.ComponentModel.DataAnnotations;
using static VAndDCargoes.Common.EntitiesValidations.Truck;

namespace VAndDCargoes.Web.ViewModels.Truck;

public class TruckAllViewModel
{
	[Required]
	public string Id { get; set; } = null!;

	[Required]
	[StringLength(MakeMaxLength, MinimumLength = MakeMinLength)]
	public string Make { get; set; } = null!;

	[Required]
	[StringLength(ModelMaxLength, MinimumLength = ModelMinLength)]
	public string Model { get; set; } = null!;

	[Range(0, int.MaxValue)]
	public int FuelCapacity { get; set; }

	[Range(0, int.MaxValue)]
	public int TravelledDistance { get; set; }
}

