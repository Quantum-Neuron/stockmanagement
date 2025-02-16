using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using stockmanagementapi.Extensions;
using stockmanagementapi.Models.Users;

namespace stockmanagementapi.Services.UserServices
{
	public class UserService : IUserService
	{
		private readonly SignInManager<User> signInManager;
		private readonly IMapper mapper;
		private readonly IHttpContextAccessor httpContextAccessor;
		private readonly ILogger<UserService> logger;

		public UserService(SignInManager<User> signInManager, IMapper mapper, IHttpContextAccessor httpContextAccessor, ILogger<UserService> logger)
		{
			this.signInManager = signInManager;
			this.mapper = mapper;
			this.httpContextAccessor = httpContextAccessor;
			this.logger = logger;
		}

		public async Task<IResult> RegisterUser(RegisterUser registerUser)
		{
			try
			{
				var user = mapper.Map<User>(registerUser);
				user.UserName = registerUser.Email;

				var result = await signInManager.UserManager
					.CreateAsync(user, registerUser.Password)
					.ConfigureAwait(false);

				if (!result.Succeeded)
				{
					var errors = result.Errors.Select(e => new { e.Code, e.Description });
					return Results.BadRequest(new { Errors = errors });
				}

				return Results.Ok();
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Failed to register the user.");
				throw new InvalidOperationException($"Failed to register the user: {ex.Message}");
			}
		}

		public async Task<IResult> LogoutUser()
		{
			try
			{
				await signInManager.SignOutAsync().ConfigureAwait(false);

				return Results.NoContent();
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Failed to log the user out.");
				throw new InvalidOperationException($"Failed to log the user out: {ex.Message}");
			}
		}

		public async Task<IResult> GetUserInfo()
		{
			try
			{
				var httpContextuser = httpContextAccessor.HttpContext?.User;
				if (httpContextuser?.Identity?.IsAuthenticated != true) return Results.NoContent();

				var user = await signInManager.UserManager
					.GetUserByEmail(httpContextuser);

				if (user == null) return Results.Unauthorized();

				return Results.Ok(
					new
					{
						user.FirstName,
						user.LastName,
						user.Email
					}
				);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Failed to get the user info.");
				throw new InvalidOperationException($"Failed to get the user info: {ex.Message}");
			}
		}

		public IResult GetAuthenticationStatus()
		{
			var httpContextuser = httpContextAccessor.HttpContext?.User;
			return Results.Ok(new { IsAuthenticated = httpContextuser?.Identity?.IsAuthenticated ?? false });
		}
  }
}
