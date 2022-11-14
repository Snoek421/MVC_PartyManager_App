using CSnoek9141_Assignment2.Entities;
using CSnoek9141_Assignment2.Models;
using CSnoek9141_Assignment2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSnoek9141_Assignment2.Controllers
{
    public class ManageController : Controller
    {
        private PartyDbContext _partyDbContext;
        private IEmailService _emailSenderService;
        public ManageController(PartyDbContext partyDbContext, IEmailService emailSenderService)
        {
            _partyDbContext = partyDbContext;
            _emailSenderService = emailSenderService; //singleton service to handle all email sending logic
        }

        [HttpGet("/party/{id}/manage")]
        public IActionResult ManageParty(int id)
        {
            PartyViewModel model = new PartyViewModel(); //make new viewmodel to send to the page
            model = GenerateManageViewModel(id); //fill some of the viewmodel properties with method
            model.NewInvitation = new Invitation(id); //make new invitation for inviting more people

            return View("Manage", model); //return manage page view and send viewmodel to the page
        }


        [HttpPost("/party/{id}/manage")]
        public IActionResult AddInvite(PartyViewModel model)
        {
            if(ModelState.IsValid) //if invitation is valid
            {
                _partyDbContext.Invitations.Add(model.NewInvitation); //add invitation to invitations table and save changes to tables
                _partyDbContext.SaveChanges();
                TempData["alert"] = "Invitation added successfully."; //tempdata for success alert

                return RedirectToAction("ManageParty", new { id = model.ActivePartyID });
            }
            else
            {
                model = GenerateManageViewModel(model.ActivePartyID); //if invitation isn't valid, repopulate missing viewmodel components and send back to the page
                return View("Manage", model);
            }
        }


        [HttpPost("party/{id}/manage/invites-sent")]
        public IActionResult SendInvitations(PartyViewModel model)
        {
            int numberOfEmails = 0; //count emails sent, to show after success
            model.Invitations = _partyDbContext.Invitations.Include(i => i.Party).Where(i => i.PartyID == model.ActivePartyID).ToList(); //get invitations list for selected party
            foreach (Invitation invitation in model.Invitations)
            {
                
                if (invitation.InviteStatus == InviteStatus.InviteNotSent) //only run code for emails with status InviteNotSent
                {
                    _emailSenderService.SendEmail(invitation); //run email sending method on the current invitation in the loop
                    invitation.InviteStatus = InviteStatus.InviteSent; //change status to sent
                    _partyDbContext.Invitations.Update(invitation); //update and save the invitations table to correct the sent status
                    _partyDbContext.SaveChanges();
                    numberOfEmails++; //increase number of successfully sent emails
                }
            }

            TempData["alert"] = numberOfEmails + " emails sent"; //tempdata for success alert

            return RedirectToAction("ManageParty", new { id = model.ActivePartyID }); //redirect to prevent refresh from trying to send emails again
        }


        /// <summary>
        /// Generates the viewmodel for the party matching the partyID that is provided
        /// </summary>
        /// <param name="id">The PartyID representing the record of the chosen party</param>
        /// <returns>Viewmodel with populated ActivePartyID, ActiveParty, and Invitations list for the given party</returns>
        private PartyViewModel GenerateManageViewModel(int id)
        {
            PartyViewModel model = new PartyViewModel();
            model.ActivePartyID = id; //set easy-to-use ID to the correct ID
            model.ActiveParty = _partyDbContext.Parties.Find(id); //return party matching the given ID
            model.Invitations = model.ActiveParty.Invitations; //separate selected party's invitation list into its own object
            return model;
        }
    }
}
