using CSnoek9141_Assignment2.Entities;

namespace CSnoek9141_Assignment2.Services
{
	public interface IEmailService
	{
		public void SendEmail(Invitation invite);
	}
}
