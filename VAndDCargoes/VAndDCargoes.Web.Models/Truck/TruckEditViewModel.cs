using System.ComponentModel.DataAnnotations;
using static VAndDCargoes.Common.EntitiesValidations.Truck;

namespace VAndDCargoes.Web.ViewModels.Truck;

public class TruckEditViewModel
{
    [Required]
    [StringLength(MakeMaxLength, MinimumLength = MakeMinLength)]
    public string Make { get; set; } = null!;

    [Required]
    [StringLength(ModelMaxLength, MinimumLength = ModelMinLength)]
    public string Model { get; set; } = null!;

    [Required]
    [StringLength(RegistrationNumberMaxLength, MinimumLength = RegistrationNumberMinLength)]
    [Display(Name = "Registration number / VIN")]
    public string RegistrationNumber { get; set; } = null!;

    [Range(0, 4)]
    [Display(Name = "Truck condition")]
    public int Condition { get; set; }

    [Range(0, int.MaxValue)]
    [Display(Name = "Travelled distance")]
    public int TravelledDistance { get; set; }

    [Range(0, 2000, ErrorMessage = "Invalid fuel capacity!")]
    [Display(Name = "Fuel capacity")]
    public int FuelCapacity { get; set; }

    [Required]
    [Display(Name = "Produced on (MM/dd/yyyy)")]
    public string CreatedOn { get; set; } = null!;

    [Required]
    [Url]
    [Display(Name = "Image url / (link)")]
    public string ImageUrl { get; set; } = null!;
}

