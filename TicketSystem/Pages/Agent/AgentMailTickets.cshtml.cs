using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketSystem.Services;

namespace TicketSystem.Pages.Agent
{
    [Authorize(Roles = "Admin, Agent")]
    public class AgentMailTicketsModel : PageModel
    {
        //Email service binnenhalen
        private EmailReceiver m_emailReceiver;

        public List<Models.MailTicket> Emails { get; set; }

        //iets van het object MailTicket
        public List<Models.MailTicket> MailTickets { get; set; }
        public Models.MailTicket MailTicket { get; set; }

        public int NewMailTickets { get; set; }

        public AgentMailTicketsModel(EmailReceiver emailReceiver)
        {
            m_emailReceiver = emailReceiver;
        }
        public void OnGet()
        {

            MailTickets = m_emailReceiver.GetMailTickets();
        }

        public IActionResult OnPostFetch()
        {
            Emails = m_emailReceiver.DownloadMessages();
            MailTickets = m_emailReceiver.GetMailTickets();

            return Page();
        }

        //Delete ticket
        public IActionResult OnPost(int id)
        {
            MailTicket = m_emailReceiver.DeleteMailTicket(id);

            return RedirectToPage("../Tickets/MyTickets");

        }
    }
}