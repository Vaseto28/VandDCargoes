using VAndDCargoes.Web.ViewModels.Truck.Enumerations;
using static VAndDCargoes.Common.GeneralConstants;

namespace VAndDCargoes.Web.ViewModels.Truck;

public class TruckQueryAllModel
{
	public TruckQueryAllModel()
	{
		this.Trucks = new HashSet<TruckAllViewModel>();

        this.CurrentPage = DefaultPage;
		this.TrucksPerPage = DefaultTrucksPerPage;
	}

	public string? Keyword { get; set; }

	public int CurrentPage { get; set; }

	public int TrucksPerPage { get; set; }

	public TrucksOrdering TrucksOrdering { get; set; }

	public virtual IEnumerable<TruckAllViewModel> Trucks { get; set; }
}

