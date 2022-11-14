using System.ComponentModel.DataAnnotations;

namespace CSnoek9141_Assignment2.Entities
{
	public class Party
	{
		public int PartyID { get; set; }

		[Required(ErrorMessage = "Please enter a description for the party")]
		public string? Description { get; set; }

		[Required(ErrorMessage = "Please enter a date for the party")]
		public DateTime? EventDate { get; set; }

		public string? Location { get; set; }

		public virtual ICollection<Invitation>? Invitations { get; set; } //navigation property

	}
}
