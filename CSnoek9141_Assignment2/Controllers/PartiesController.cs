using CSnoek9141_Assignment2.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSnoek9141_Assignment2.Controllers
{
    public class PartiesController : Controller
    {
        private PartyDbContext _partyDbContext;
        public PartiesController(PartyDbContext partyDbContext)
        {
            _partyDbContext = partyDbContext;
        }

        [HttpGet("/parties")]
        public IActionResult GetParties()
        {

            List<Party> parties = _partyDbContext.Parties.OrderByDescending(p => p.EventDate).ToList(); //return list of parties from database in order of dates

            return View("PartyList", parties); //hand that list to the page
        }

        [HttpGet("/party/{id}/edit")]
        public IActionResult GetEditForm(int id)
        {
            Party activeParty = _partyDbContext.Parties.Find(id); //find the party with the given ID in the database

            return View("Edit", activeParty); //hand that party to the page
        }

        [HttpPost("/party/{id}/edit")]
        public IActionResult EditParty(Party activeParty)
        {
            if (ModelState.IsValid) //if party data is valid then update party data in database and save changes
            {
                _partyDbContext.Parties.Update(activeParty);
                _partyDbContext.SaveChanges();

                return RedirectToAction("ManageParty", "Manage", new { id = activeParty.PartyID }); //then redirect to the manage page
            }
            else
            {
                return View("Edit", activeParty); //otherwise return to the edit page to show errors
            }
        }


        [HttpGet("/party/new-party")]
        public IActionResult GetNewPartyForm()
        {
            Party newParty = new Party(); //make new party instance
            return View("Create", newParty); //send new party instance to the create page
        }

        [HttpPost("/party/new-party")]
        public IActionResult AddNewParty(Party newParty)
        {
            if (ModelState.IsValid) //if party data is valid then add party record to database and save changes
            {
                _partyDbContext.Parties.Add(newParty);
                _partyDbContext.SaveChanges();
                Party activeParty = _partyDbContext.Parties.Find(newParty.PartyID); //then retrieve the fully made party instance from database, to send to the manage page

                return RedirectToAction("ManageParty", "Manage", new { id = activeParty.PartyID });
            }
            else
            {
                return View("Create", newParty); //otherwise return to the create page to show errors
            }
        }

        [HttpGet("/parties/{PartyID}/invitations/{InvitationID}")]
        public IActionResult GetInviteResponseForm([FromRoute] int invitationID)
        {
            Invitation invitation = _partyDbContext.Invitations.Include(i => i.Party).Where(i => i.InvitationID == invitationID).FirstOrDefault(); //retrieve invitation instance from database to hand to page, with attached party info
            return View("Response", invitation); //send to reponse page to show accurate data and have its status updated
        }

        [HttpPost()]
        public IActionResult RespondToInvite(Invitation invitation)
        {
            string status = "";
            _partyDbContext.Invitations.Update(invitation); //update invitation's status with yes or no input from page then save changes
            _partyDbContext.SaveChanges();
            if (invitation.InviteStatus == InviteStatus.RespondedYes) //Assign yes or no to a string to send to the thank you page
            {
                status = "yes";
            }
            else if (invitation.InviteStatus == InviteStatus.RespondedNo)
            {
                status = "no";
            }

            return RedirectToAction("ThankYou", "Parties", new { status = status }); //redirect to thank you action, with yes or no as the route value
        }

        [HttpGet("/thank-you/{status}")]
        public IActionResult ThankYou([FromRoute] string status) //retrieve status string from the route value
        {
            string responseMessage = ""; //prepare string to be sent to and show on the page, matching the yes or no response
            if (status == "yes")
            {
                responseMessage = "We are thrilled that you will be joining us!";
            }
            else if (status == "no")
            {
                responseMessage = "We are sad to hear that you won't be joining us!";
            }

            return View("ThankYou", responseMessage); //hand the response message to the page
        }

    }
}
