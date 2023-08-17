using System.ComponentModel.DataAnnotations;
using VAndDCargoes.Web.ViewModels.Trailer;
using static VAndDCargoes.Common.EntitiesValidations.Repairment;

namespace VAndDCargoes.Web.ViewModels.Repairment;

public class CreateRepairmentOfTrailerViewModel
{
    public CreateRepairmentOfTrailerViewModel()
    {
        this.Trailers = new HashSet<TrailerAllViewModel>();
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
    [Display(Name = "Trailer")]
    public Guid TrailerId { get; set; }

    public IEnumerable<TrailerAllViewModel> Trailers { get; set; } = null!;
}

