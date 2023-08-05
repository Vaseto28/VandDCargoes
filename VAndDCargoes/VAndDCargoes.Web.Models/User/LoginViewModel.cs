using System.ComponentModel.DataAnnotations;
using static VAndDCargoes.Common.EntitiesValidations.User;

namespace VAndDCargoes.Web.ViewModels.User;

public class LoginViewModel
{
	[Required]
	[StringLength(UsernameMaxLength, MinimumLength = UsernameMinLength)]
	public string Username { get; set; } = null!;

	[Required]
	[DataType(DataType.Password)]
	public string Password { get; set; } = null!;

	[Display(Name = "Remember me?")]
	public bool RememberMe { get; set; }

	public string? ReturnUrl { get; set; }
}

