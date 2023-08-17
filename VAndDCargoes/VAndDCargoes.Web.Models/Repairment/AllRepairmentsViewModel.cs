using System.ComponentModel.DataAnnotations;
using static VAndDCargoes.Common.EntitiesValidations.Repairment;

namespace VAndDCargoes.Web.ViewModels.Repairment;

public class AllRepairmentsViewModel
{
	public int Id { get; set; }

    [Required]
    [Display(Name = "Repairment code number")]
    public int Type { get; set; }

    [Required]
    [StringLength(DescriptionMaxLenght, MinimumLength = DescriptionMinLength)]
    public string Description { get; set; } = null!;

    public int Quantity { get; set; }

    public int Cost { get; set; }

    [Required]
    [Display(Name = "Truck")]
    public string TruckId { get; set; } = null!;
}

