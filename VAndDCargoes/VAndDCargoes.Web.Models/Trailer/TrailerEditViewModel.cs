using System.ComponentModel.DataAnnotations;
using static VAndDCargoes.Common.EntitiesValidations.Trailer;

namespace VAndDCargoes.Web.ViewModels.Trailer;

public class TrailerEditViewModel
{
    [Display(Name = "Trailer Capacity")]
    [Range(CapacityMinValue, CapacityMaxValue)]
	public int Capacity { get; set; }

    [Display(Name = "Trailer condition")]
    public int Condition { get; set; }

    [Display(Name = "Trailer category")]
    public int Category { get; set; }

    [Display(Name = "Trailer dementions")]
    public int Dementions { get; set; }

    [Required]
    [Url]
    [Display(Name = "Image url / (link)")]
    public string ImageUrl { get; set; } = null!;
}

