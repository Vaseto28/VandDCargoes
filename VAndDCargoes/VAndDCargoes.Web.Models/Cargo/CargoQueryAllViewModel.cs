using System.ComponentModel.DataAnnotations;
using static VAndDCargoes.Common.GeneralConstants;

namespace VAndDCargoes.Web.ViewModels.Cargo;

public class CargoQueryAllViewModel
{
    public CargoQueryAllViewModel()
    {
        this.Cargoes = new HashSet<CargoAllViewModel>();

        this.CurrentPage = DefaultPage;
        this.CargoesPerPage = DefaultCargoesPerPage;
    }

    public string? Keyword { get; set; }

    public int CurrentPage { get; set; }

    [Display(Name = "Cargoes per page:")]
    public int CargoesPerPage { get; set; }

    [Display(Name = "Order cargoes by:")]
    public CargoesOrdering CargoesOrdering { get; set; }

    public virtual IEnumerable<CargoAllViewModel> Cargoes { get; set; }
}

