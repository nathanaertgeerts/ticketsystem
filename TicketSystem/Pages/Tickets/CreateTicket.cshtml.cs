using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Data;
using TicketSystem.Models;
using TicketSystem.Services;

namespace TicketSystem.Pages.Private
{
    public class CreateTicketModel : PageModel
    {
        public class InputModel
        {
            public string TicketSubject { get; set; }
            public string TicketDetails { get; set; }
            public string TicketRequestor { get; set; }
            public Models.Priority TicketPriority { get; set; }
            public Models.Status TicketStatus { get; set; }

            public Models.Product TicketProduct { get; set; }

            public string Category { get; set; }

        }

        [BindProperty]
        public InputModel Input { get; set; }

        public List<Categories> CategoryList { get; set; }

        //Ticket service binnenhalen
        private TicketService m_ticketService;
        private UserManager<ApplicationUser> m_userManager;
        private ApplicationDbContext m_context;
        //Email sender interface
        private readonly IMailManager m_mailmanager;


        public CreateTicketModel(TicketService ticketService, UserManager<ApplicationUser> userManager, IMailManager mailManager, ApplicationDbContext context)
        {
            m_ticketService = ticketService;
            m_userManager = userManager;
            m_mailmanager = mailManager;
            m_context = context;
        }

        public void OnGet()
        {
            CategoryList = m_ticketService.GetCategories();
        }
        //interface actie - resultaat onPost (submit roept die aan)
        public IActionResult OnPost()
        {
            

            var ticket = new Models.Ticket()
            {
                TicketSubject = Input.TicketSubject,
                TicketDetails = Input.TicketDetails,
                TicketRequestor = User.Identity.Name,
                TicketDate = DateTime.Now,
                PriorityType = Input.TicketPriority,
                StatusType = Models.Status.Open,
                ProductType = Input.TicketProduct,
            };

            m_ticketService.AddTicket(ticket);
            m_ticketService.AddCategoryToTicket(ticket.TicketId, Input.Category);

            var user = new ApplicationUser { UserName = Input.TicketRequestor, Email = Input.TicketRequestor };
            var result = m_userManager.CreateAsync(user).Result;
            if (result.Succeeded)
            {
                //add user to role
                var test2 = m_userManager.AddToRoleAsync(user, "Customer").Result;
                //add notifcation settings
                var userNotifications = m_userManager.Users.Include(x => x.Notifications).FirstOrDefault(x => x.UserName == user.UserName);
                userNotifications.Notifications.Add(new Models.Notifications()
                {
                    TicketCreated = true,
                    TicketUpdate = true,
                    NewArticle = true,
                    NewDocument = true
                });
                m_context.SaveChanges();
                //new account has been created
                //stuur een Email confirmatie mail  ....
                var code = m_userManager.GenerateEmailConfirmationTokenAsync(user).Result;
                var callbackUrl = Url.EmailConfirmationNewTicketLink(user.Id, code, Request.Scheme);

                m_mailmanager.SendEmailConfirmationAsync(Input.TicketRequestor, callbackUrl, Input.TicketRequestor);

                // zorg ervoor dat de gebruiker hier ook zijn passwoord kan setten
            }
            else
            {
                //account already in DB
                //stuur mail dat er een ticket is aangemaakt
                SendMailTicketCreate(ticket.TicketId);
            }

           

            //na post van ticket, redirect naar my ticket lijst
            return RedirectToPage("../Tickets/MyTickets");
        }

        public void SendMailTicketCreate(int Id)
        {
            var callbackUrl = Url.TicketLink(Request.Scheme);
            m_mailmanager.SendEmailTicketCreate(Input.TicketRequestor, callbackUrl, Id);
        }

    }
}