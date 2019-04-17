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
    public class AgentTicketsModel : PageModel
    {
        //Lijst van alle tickets
        public List<Models.Ticket> Tickets { get; set; }

        //iets van het object ticket
        public Models.Ticket Ticket { get; set; }

        //Ticket service binnenhalen
        private TicketService m_ticketService;
        public AgentTicketsModel(TicketService ticketService)
        {
            m_ticketService = ticketService;
        }

        //onGet alle tickets binnenhalen
        public void OnGet()
        {
            Tickets = m_ticketService.GetTickets();
        }
    }
}