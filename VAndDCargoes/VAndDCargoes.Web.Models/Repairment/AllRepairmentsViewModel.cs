using System.ComponentModel.DataAnnotations;
using static VAndDCargoes.Common.EntitiesValidations.Repairment;

namespace VAndDCargoes.Web.ViewModels.Repairment;

public class AllRepairmentsViewModel
{
	public int Id { get; set; }

    [Required]
    [Display(Name = "Repairment type")]
    [StringLength(TypeMaxLength, MinimumLength = TypeMinLength)]
    public string Type { get; set; } = null!;

    [Required]
    [StringLength(DescriptionMaxLenght, MinimumLength = DescriptionMinLength)]
    public string Description { get; set; } = null!;

    [Range(CostMinValue, CostMaxValue)]
    public int Cost { get; set; }

    [Required]
    [Display(Name = "Truck")]
    public string TruckId { get; set; } = null!;
}

