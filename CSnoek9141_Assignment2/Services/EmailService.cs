using CSnoek9141_Assignment2.Entities;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace CSnoek9141_Assignment2.Services
{
	public class EmailService : IEmailService
	{
		private static MailAddress fromAddress = new MailAddress("yourGmail@gmail.com", "PartyApp");
		private SmtpClient smtpClient = new SmtpClient("smtp.gmail.com") //set mail server provider to gmail
		{
			Port = 587,
			UseDefaultCredentials = false,
			DeliveryMethod = SmtpDeliveryMethod.Network,
			Credentials = new NetworkCredential(fromAddress.Address, "yourGeneratedAppPassword"), //no spaces in app password
			EnableSsl = true
		};

		public void SendEmail(Invitation invite)
		{


			MailMessage inviteEmail = new MailMessage() //construct the email message with from address, subject, and body text based on the provided invitation
			{
				From = fromAddress,
				Subject = $"You have been invited to the \"{invite.Party.Description}\" party!",
				Body = $"<h1>Hello {invite.GuestName}:</h1>" +
				$"<p>You have been invited to the \"{invite.Party.Description}\" at {invite.Party.Location} on {invite.Party.EventDate?.ToString("d")}!</p>" +
				$"<p>We would be thrilled to have you so please <a href=\"https://localhost:7141/parties/{invite.PartyID}/invitations/{invite.InvitationID}\">let us know</a> if you can as soon as possible!</p>" +
				$"<p>Sincerely,</p>" +
				$"<p>The Party Manager App</p>",
				IsBodyHtml = true
			};
			MailAddress toAddress = new MailAddress(invite.GuestEmail, invite.GuestName);
			inviteEmail.To.Add(toAddress); //add the name and email address the message is being sent to
			smtpClient.Send(inviteEmail); //try sending the email
		}
	}
}
