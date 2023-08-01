using System.ComponentModel.DataAnnotations;
using VAndDCargoes.Web.ViewModels.Trailer.Enumerations;
using static VAndDCargoes.Common.GeneralConstants;

namespace VAndDCargoes.Web.ViewModels.Trailer;

public class TrailerQueryAllViewModel
{
    public TrailerQueryAllViewModel()
    {
        this.CurrentPage = DefaultPage;
        this.TrailersPerPage = DefaultTrailersPerPage;

        this.Trailers = new HashSet<TrailerAllViewModel>();
    }

    public int CurrentPage { get; set; }

    [Display(Name = "Trailers per page:")]
    public int TrailersPerPage { get; set; }

    [Display(Name = "Order trailers by:")]
    public TrailersOrdering TrailersOrdering { get; set; }

    public virtual IEnumerable<TrailerAllViewModel> Trailers { get; set; }
}

