using Microsoft.AspNetCore.Mvc;
using stockmanagementapi.Models.Users;

namespace stockmanagementapi.Services.UserServices
{
	public interface IUserService
	{
		Task<IResult> RegisterUser(RegisterUser registerUser);

		Task<IResult> LogoutUser();

		Task<IResult> GetUserInfo();

		IResult GetAuthenticationStatus();
  }
}
