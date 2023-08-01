using System.ComponentModel.DataAnnotations;
using static VAndDCargoes.Common.EntitiesValidations.Driver;

namespace VAndDCargoes.Web.ViewModels.Driver;

public class BecomeDriverViewModel
{
	[Required]
	[StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]
	public string FirstName { get; set; } = null!;

	[Required]
	[StringLength(SecondNameMaxLength, MinimumLength = SecondNameMinLength)]
	public string SecondName { get; set; } = null!;

	[Required]
	[StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength)]
	public string LastName { get; set; } = null!;

	[Phone]
	[StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
	public string PhoneNumber { get; set; } = null!;

	public int Age { get; set; }
}

