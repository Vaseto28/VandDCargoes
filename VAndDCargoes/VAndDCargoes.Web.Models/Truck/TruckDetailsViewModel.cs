using System.ComponentModel.DataAnnotations;
using static VAndDCargoes.Common.EntitiesValidations.Truck;

namespace VAndDCargoes.Web.ViewModels.Truck;

public class TruckDetailsViewModel
{
	[Required]
	public string Id { get; set; } = null!;

	[Required]
	[StringLength(MakeMaxLength, MinimumLength = MakeMinLength)]
	public string Make { get; set; } = null!;

	[Required]
	[StringLength(MakeMaxLength, MinimumLength = MakeMinLength)]
	public string Model { get; set; } = null!;

	[Required]
	[StringLength(RegistrationNumberMaxLength, MinimumLength = RegistrationNumberMinLength)]
	[Display(Name = "Registration number / VIN")]
	public string RegistrationNumber { get; set; } = null!;

	[Required]
	[Display(Name = "Travelled distance (km)")]
	public int TravelledDistance { get; set; }

	[Required]
	[Display(Name = "Fuel capacity (litres)")]
	public int FuelCapacity { get; set; }

	[Required]
	public string Condition { get; set; } = null!;

	[Required]
	[Display(Name = "Created on")]
	public string CreatedOn { get; set; } = null!;

    [Required]
	[Display(Name = "Name of the creator")]
    public string CreatorName { get; set; } = null!;

    [Required]
	[Display(Name = "Name of the driver")]
    public string DriverName { get; set; } = null!;

    [Required]
    public string Trailer { get; set; } = null!;

	[Required]
	public string CreatorEmail { get; set; } = null!;
}

