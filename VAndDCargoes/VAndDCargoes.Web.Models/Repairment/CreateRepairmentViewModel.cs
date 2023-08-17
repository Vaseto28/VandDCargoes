using System.ComponentModel.DataAnnotations;
using VAndDCargoes.Web.ViewModels.Truck;
using static VAndDCargoes.Common.EntitiesValidations.Repairment;

namespace VAndDCargoes.Web.ViewModels.Repairment;

public class CreateRepairmentViewModel
{
	public CreateRepairmentViewModel()
	{
		this.Trucks = new HashSet<TruckAllViewModel>();
	}

	[Required]
	[Display(Name = "Repairment type")]
	public int Type { get; set; }

	[Required]
	[StringLength(DescriptionMaxLenght, MinimumLength = DescriptionMinLength)]
	public string Description { get; set; } = null!;

	public int Quantity { get; set; }

	public int Cost { get; set; }

	[Required]
    [Display(Name = "Truck")]
    public Guid TruckId { get; set; }

	public IEnumerable<TruckAllViewModel> Trucks { get; set; } = null!;
}

