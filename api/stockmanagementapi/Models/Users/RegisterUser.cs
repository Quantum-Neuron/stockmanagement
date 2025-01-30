using System.ComponentModel.DataAnnotations;

namespace stockmanagementapi.Models.Users
{
	public class RegisterUser
	{
		[Required]
		public string FirstName { get; set; } = string.Empty;

		[Required]
		public string LastName { get; set; } = string.Empty;

		[Required]
		public string Email { get; set; } = string.Empty;

		[Required]
		public string Password { get; set; } = string.Empty;
	}
}
