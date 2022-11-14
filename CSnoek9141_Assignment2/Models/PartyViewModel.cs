using CSnoek9141_Assignment2.Entities;
using System.Net;

namespace CSnoek9141_Assignment2.Models;
public class PartyViewModel
{
	public int ActivePartyID { get; set; } //easier method of passing the party's ID around between pages than passing the whole party, or accessing it through an invitation
	public Party? ActiveParty { get; set; } //used to hold and pass the party being edited or managed

	public Invitation? NewInvitation { get; set; } //used to hold and pass new invitations being added to the active party
	public ICollection<Invitation>? Invitations { get; set; } //used to hold and pass invitations for sending emails
}
