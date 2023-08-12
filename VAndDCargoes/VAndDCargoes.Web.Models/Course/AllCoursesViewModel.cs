using System.ComponentModel.DataAnnotations;
using static VAndDCargoes.Common.EntitiesValidations.Course;

namespace VAndDCargoes.Web.ViewModels.Course;

public class AllCoursesViewModel
{
	[Required]
	public string Id { get; set; } = null!;

	[Required]
	[Display(Name = "Departure city")]
	[StringLength(DepartureCityMaxLength, MinimumLength = DepartureCityMinLength)]
	public string DepartureCity { get; set; } = null!;

	[Required]
	[Display(Name = "Arrival city")]
	[StringLength(ArrivalCityMaxLength, MinimumLength = ArrivalCityMinLength)]
	public string ArrivalCity { get; set; } = null!;

	[Range(DistanceMinValue, DistanceMaxValue)]
	public int Distance { get; set; }

	public decimal Reward { get; set; }

	[Required]
	public string Truck { get; set; } = null!;

	[Required]
	public string Cargo { get; set; } = null!;
}

