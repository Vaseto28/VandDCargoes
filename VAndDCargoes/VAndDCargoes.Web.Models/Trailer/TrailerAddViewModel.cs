using System.ComponentModel.DataAnnotations;
using static VAndDCargoes.Common.EntitiesValidations.Trailer;

namespace VAndDCargoes.Web.ViewModels.Trailer;

public class TrailerAddViewModel
{
    [Range(CapacityMinValue, CapacityMaxValue)]
    [Display(Name = "Trailer capacity")]
    public int Capacity { get; set; }

    [Display(Name = "Trailer condition")]
    public int Condition { get; set; }

    [Display(Name = "Trailer category")]
    public int Category { get; set; }

    [Display(Name = "Trailer dementions")]
    public int Dementions { get; set; }
}

