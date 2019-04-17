using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketSystem.Data;
using TicketSystem.Services;

namespace TicketSystem.Pages.Tickets
{
    [Authorize(Roles = "Admin, Agent")]
    public class TicketConverterModel : PageModel
    {
        public class InputModel
        {
            public string TicketSubject { get; set; }
            public string TicketDetails { get; set; }
            public string TicketRequestor { get; set; }
            public Models.Priority TicketPriority { get; set; }
            public Models.Status TicketStatus { get; set; }
            public Models.Product TicketProduct { get; set; }

            public int MailId { get; set; }

            public bool Delete { get; set; }

        }

        [BindProperty]
        public InputModel Input { get; set; }

        //Ticket service binnenhalen
        private TicketService m_ticketService;
        private UserManager<ApplicationUser> m_userManager;
        //Email service binnenhalen
        private EmailReceiver m_emailReceiver;
        //Email sender interface
        private readonly IMailManager m_mailmanager;

        //iets van het object MailTicket
        public List<Models.MailTicket> MailTickets { get; set; }
        public Models.MailTicket MailTicket { get; set; }


        public TicketConverterModel(TicketService ticketService, UserManager<ApplicationUser> userManager, EmailReceiver emailReceiver, IMailManager mailManager)
        {
            m_ticketService = ticketService;
            m_userManager = userManager;
            m_emailReceiver = emailReceiver;
            m_mailmanager = mailManager;
        }

        public IActionResult OnGet(int id)
        {
            MailTicket = m_emailReceiver.GetMailById(id);
            Input = new InputModel()
            {
                MailId = id
            };

            return Page();

        }
        public IActionResult OnPostCreate(int id)
        {
            var ticket = new Models.Ticket()
            {
                TicketSubject = Input.TicketSubject,
                TicketDetails = Input.TicketDetails,
                TicketRequestor = User.Identity.Name,
                TicketDate = DateTime.Now,
                PriorityType = Input.TicketPriority,
                StatusType = Models.Status.Open,
                ProductType = Input.TicketProduct
            };

            m_ticketService.AddTicket(ticket);

            var user = new ApplicationUser { UserName = Input.TicketRequestor, Email = Input.TicketRequestor };
            var result = m_userManager.CreateAsync(user).Result;
            if (result.Succeeded)
            {
                //add user to role
                var test2 = m_userManager.AddToRoleAsync(user, "Customer").Result;
                //new account has been created
                //stuur een Email confirmatie mail  ....
                var code = m_userManager.GenerateEmailConfirmationTokenAsync(user).Result;
                var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                m_mailmanager.SendEmailConfirmationAsync(Input.TicketRequestor, callbackUrl, Input.TicketRequestor);

                // zorg ervoor dat de gebruiker hier ook zijn passwoord kan setten
            }
            else
            {
                //account already in DB
                //stuur mail dat er een ticket is aangemaakt
                SendMailTicketCreate(ticket.TicketId);
            }

            if (Input.Delete == true)
            {
                m_emailReceiver.DeleteMailTicket(id);
            }

            return Redirect("../../Tickets/MyTickets");
        }

        public void SendMailTicketCreate(int id)
        {
            var callbackUrl = Url.TicketLink(Request.Scheme);
            m_mailmanager.SendEmailTicketCreate(Input.TicketRequestor, callbackUrl, id);
        }
    }
}