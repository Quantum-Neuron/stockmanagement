using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using stockmanagementapi.Models.Users;
using System.Security.Authentication;
using System.Security.Claims;

namespace stockmanagementapi.Extensions
{
	public static class ClaimsPrincipleExtensions
	{
		public static async Task<User> GetUserByEmail(this UserManager<User> userManager, ClaimsPrincipal user)
		{
			var returnUser = await userManager.Users
				.FirstOrDefaultAsync(u => u.Email == user.GetEmail())
				.ConfigureAwait(false);

			if (returnUser == null) throw new AuthenticationException("User not found");

			return returnUser;
		}

		public static string GetEmail(this ClaimsPrincipal user)
		{
			return user.FindFirstValue(ClaimTypes.Email) 
				?? throw new AuthenticationException("Email claim not found");
		}
	}
}
