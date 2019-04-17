using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketSystem.Data;
using TicketSystem.Models;
using TicketSystem.Services;

namespace TicketSystem.Pages.Tickets
{
    public class MyTicketsModel : PageModel
    {
        //Lijst van alle tickets
        public List<Models.Ticket> Tickets { get; set; }
        // Lijst met open tickets
        public List<Models.Ticket> OpenTickets { get; set; }
        // Lijst met Pending tickets
        public List<Models.Ticket> PendingTickets { get; set; }

        // Lijst met Tickets by User
        public List<Models.Ticket> CurUserTickets { get; set; }

        // Lijst met Open Tickets by User
        public List<Models.Ticket> CurUserOpenTickets { get; set; }
        // Lijst met Open Tickets by User
        public List<Models.Ticket> CurUserPendingTickets { get; set; }

        //iets van het object ticket
        public Models.Ticket Ticket { get; set; }

        public class InputModel
        {
            public string TicketRequestor { get; set; }
        }
        [BindProperty]
        public InputModel Input { get; set; }

        //Ticket service binnenhalen
        private TicketService m_ticketService;

        private UserManager<ApplicationUser> m_userManager;
        public MyTicketsModel(TicketService ticketService, UserManager<ApplicationUser> userManager)
        {
            m_ticketService = ticketService;
            m_userManager = userManager;
        }

        //onGet alle tickets binnenhalen
        public void OnGet()
        {
            Tickets = m_ticketService.GetTickets();
            OpenTickets = m_ticketService.GetTicketsByStatus(Models.Status.Open);
            PendingTickets = m_ticketService.GetTicketsByStatus(Models.Status.Pending);

            CurUserTickets = m_ticketService.GetTicketsByUser(User);

            CurUserOpenTickets = m_ticketService.GetTicketsByUserAndStatus(User, Models.Status.Open);
            CurUserPendingTickets = m_ticketService.GetTicketsByUserAndStatus(User, Models.Status.Pending);
        }
        
        //Delete ticket
        public IActionResult OnPost(int id)
        {
            Ticket = m_ticketService.DeleteTicket(id);

            return RedirectToPage("../Tickets/MyTickets");

        }


    }
}