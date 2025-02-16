using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using stockmanagementapi.Models.Users;
using stockmanagementapi.Services.UserServices;

namespace stockmanagementapi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{
		private readonly IUserService userService;

		public UserController(IUserService userService)
		{
			this.userService = userService;
		}

		[HttpPost("register")]
		public Task<IResult> Register([FromBody] RegisterUser registerUser)
		{
			return this.userService.RegisterUser(registerUser);
		}

		[Authorize]
		[HttpPost("logout")]
		public Task<IResult> LogoutUser()
		{
			return this.userService.LogoutUser();
		}

		[Authorize]
		[HttpGet("user-info")]
		public Task<IResult> GetUserInfo()
		{
			return this.userService.GetUserInfo();
		}

		[HttpGet("auth-status")]
		public IResult GetAuthenticationStatus()
		{
			return this.userService.GetAuthenticationStatus();
		}
  }
}
