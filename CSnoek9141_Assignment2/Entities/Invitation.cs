using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CSnoek9141_Assignment2.Entities
{
	public class Invitation
	{
        public Invitation()
        {
            InviteStatus = InviteStatus.InviteNotSent; //on creation of invitation, set default value of invitestatus to not sent
        }
        public Invitation(int id) //a second constructor that lets you assign the partyID on creation, allowing for simpler invitation inputs on manage page
		{
			PartyID = id;
			InviteStatus = InviteStatus.InviteNotSent;
		}

		public int InvitationID { get; set; }

		[Required(ErrorMessage = "Please enter a name for the guest")] 
		public string? GuestName { get; set; }

		[Required(ErrorMessage = "Please enter an email to send the invitation to")]
		[RegularExpression(@"^([a-zA-Z0-9._%-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4})*$", ErrorMessage = "Please enter a valid email in the format 'example@domain.com'")]
		public string? GuestEmail { get; set; }

		public InviteStatus InviteStatus { get; set; }

		public int PartyID { get; set; } //foreign key
		public virtual Party? Party { get; set; } //navigation property


		/// <summary>
		/// Simple method that returns a user friendly string for each of the Enum options available for InviteStatus
		/// </summary>
		/// <returns></returns>
		public string GetStatus()
		{
			switch (this.InviteStatus)
			{
				case InviteStatus.InviteNotSent:
					return "Invitation not sent";
				case InviteStatus.InviteSent:
					return "Invitation sent";
				case InviteStatus.RespondedYes:
					return "Responded yes";
				case InviteStatus.RespondedNo:
					return "Responded no";
				default:
					return "";
			}
		}
	}
}
