using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace stockmanagementapi.Models.Users
{
	public class User : IdentityUser
	{
		[StringLength(100)]
		public string? FirstName { get; set; }

		[StringLength(100)]
		public string? LastName { get; set; }	
	}
}
