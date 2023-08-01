using System.ComponentModel.DataAnnotations;

namespace VAndDCargoes.Web.ViewModels.Trailer;

public class TrailerDetailsViewModel
{
	[Required]
	public string Id { get; set; } = null!;

	public int Capacity { get; set; }

	[Required]
	public string Category { get; set; } = null!;

	[Required]
	public string Condition { get; set; } = null!;

	[Required]
	public string Dementions { get; set; } = null!;

	[Required]
	public string Cargo { get; set; } = null!;

	[Required]
	[Display(Name = "Trailer created by: ")]
	public string CreatorEmail { get; set; } = null!;
}

