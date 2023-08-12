using System.ComponentModel.DataAnnotations;
using VAndDCargoes.Web.ViewModels.Cargo;
using VAndDCargoes.Web.ViewModels.Trailer;
using VAndDCargoes.Web.ViewModels.Truck;
using static VAndDCargoes.Common.EntitiesValidations.Course;

namespace VAndDCargoes.Web.ViewModels.Course;

public class StartCourseViewModel
{
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

	[Required]
	public IEnumerable<TruckAllViewModel> Trucks { get; set; } = new HashSet<TruckAllViewModel>();

	[Required]
	[Display(Name = "Truck")]
	public string TruckId { get; set; } = null!;

	[Required]
	public IEnumerable<TrailerAllViewModel> Trailers { get; set; } = new HashSet<TrailerAllViewModel>();

    [Required]
	[Display(Name = "Trailer")]
	public string TrailerId { get; set; } = null!;

	[Required]
	public IEnumerable<CargoAllViewModel> Cargoes { get; set; } = new HashSet<CargoAllViewModel>();

    [Required]
	[Display(Name = "Cargo")]
	public string CargoId { get; set; } = null!;
}

