using Microsoft.AspNetCore.Identity;
using stockmanagementapi.Models.Users;

namespace stockmanagementapi.Services.EmailSenderServices
{
	public class EmailSender : IEmailSender<User>
	{
		public Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
		{
			throw new NotImplementedException();
		}

		public Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			return Task.CompletedTask;
		}

		public Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
		{
			throw new NotImplementedException();
		}

		public Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
		{
			throw new NotImplementedException();
		}
	}
}
