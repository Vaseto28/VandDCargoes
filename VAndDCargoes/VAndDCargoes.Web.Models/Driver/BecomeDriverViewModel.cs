using System.ComponentModel.DataAnnotations;
using static VAndDCargoes.Common.EntitiesValidations.Driver;

namespace VAndDCargoes.Web.ViewModels.Driver;

public class BecomeDriverViewModel
{
	[Required]
	[Display(Name = "First name")]
	[StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]
	public string FirstName { get; set; } = null!;

	[Required]
	[Display(Name = "Second name")]
	[StringLength(SecondNameMaxLength, MinimumLength = SecondNameMinLength)]
	public string SecondName { get; set; } = null!;

	[Required]
	[Display(Name = "Last name")]
	[StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength)]
	public string LastName { get; set; } = null!;

	[Phone]
	[Display(Name = "Phone number")]
	[StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
	public string PhoneNumber { get; set; } = null!;

	public int Age { get; set; }
}

