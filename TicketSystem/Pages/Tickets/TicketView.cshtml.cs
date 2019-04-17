using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Data;
using TicketSystem.Models;
using TicketSystem.Services;

namespace TicketSystem.Pages.Tickets
{
    public class TicketViewModel : PageModel
    {
        public class InputModel
        {
            public Status TicketStatus { get; set; }
            public Priority TicketPriority { get; set; }

            public int TicketId { get; set; }

            public string ReplyContent { get; set; }

            public int ReplyId { get; set; }

            public string TicketRequestor { get; set; }

            public string WorkItemID { get; set; }

            public int BugId { get; set; }
            public string Category { get; set; }

        }

        [BindProperty]
        public InputModel Input { get; set; }

        //iets van het object
        public Models.Ticket Ticket { get; set; }
        public Models.Reply Reply { get; set; }
        public List<APIKey> ApiKey { get; set; }
        public ApplicationUser UserDetails { get; set; }
        public Companies CompanyDetails { get; set; }
        public List<Categories> CategoryList { get; set; }

        //Services binnenhalen
        private TicketService m_ticketService;
        private VSTSService m_VSTSService;
        private UserService m_userService;
        private UserManager<ApplicationUser> m_userManager;
        private readonly IMailManager m_mailmanager;
        private ExactOnlineService m_exactOnlineService;
        private UserInCompanyService m_userInCompanyService;
        private ApplicationDbContext m_context;

        public TicketViewModel(TicketService ticketService, UserManager<ApplicationUser> userManager, IMailManager mailManager, VSTSService vstsService, UserService userService, ExactOnlineService exactOnlineService, UserInCompanyService userInCompanyService, ApplicationDbContext context)
        {
            m_ticketService = ticketService;
            m_userManager = userManager;
            m_mailmanager = mailManager;
            m_VSTSService = vstsService;
            m_userService = userService;
            m_exactOnlineService = exactOnlineService;
            m_userInCompanyService = userInCompanyService;
            m_context = context;
        }

        //onGet ticket by ID binnenhalen
        public IActionResult OnGet(int id)
        {
            CategoryList = m_ticketService.GetCategories();
            ApiKey = m_userService.GetApiKeys(User.Identity.Name);

            
            var APIkey = m_userManager.Users.Include(x => x.ApiKeys).FirstOrDefault(x => x.UserName == User.Identity.Name).ApiKeys.ToList();
            if (APIkey.Count() == 0)
            {
                //hier kan je nog error handling voorzien
            }
            else
            {
                foreach (var item in APIkey)
                {
                    if (item.Source == Source.VSTS)
                    {
                        var token = item.VSTStoken;
                        m_VSTSService.GetAllBugs(token);
                    }
                    else
                    {
                        //hier kan je nog error handling voorzien
                    }
                }
                
            }

            Ticket = m_ticketService.GetTicketById(id);
            Input = new InputModel()
            {
                TicketId = id
            };

            if (Ticket == null)
            { return RedirectToPage("../Tickets/MyTickets"); }


            UserDetails = m_userService.GetUserByMail(Ticket.TicketRequestor);
            if (m_userInCompanyService.GetUsersInCompany().FirstOrDefault(x => x.UserId == UserDetails.Id) == null)
            {
                Page();
            }
            else
            {
                CompanyDetails = GetCompanyDetails(UserDetails.Id);
            }
            

            return Page();

        }

        //Delete ticket
        public IActionResult OnPostDelete(int id)
        {
            Ticket = m_ticketService.DeleteTicket(id);

            return RedirectToPage("../Tickets/MyTickets");

        }

        //Update ticket
        public IActionResult OnPostUpdate(int id)
        {
           
            Ticket = m_ticketService.UpdateTicket(id, Input.TicketPriority, Input.TicketStatus);
            m_ticketService.AddCategoryToTicket(Ticket.TicketId, Input.Category);

            SendMailTicketUpdate(id);

            return RedirectToPage();
        }

        //Add reply
        public IActionResult OnPostReply(string ReplyContent)
        {
            SendMailTicketUpdate(Input.TicketId);

            m_ticketService.AddReply(User.Identity.Name, DateTime.Now, Input.ReplyContent, Input.TicketId);

            return RedirectToPage();
        }

        //Delete Reply
        public IActionResult OnPostDeleteReply(int id)
        {
            Reply = m_ticketService.DeleteReply(Input.ReplyId);

            return RedirectToPage();

        }

        //Add bug
        public IActionResult OnPostLink(string WorkItemId)
        {
            m_VSTSService.AddBug(Input.WorkItemID, Input.TicketId);

            return RedirectToPage();
        }

        public void SendMailTicketUpdate(int Id)
        {
            var callbackUrl = Url.TicketLink(Request.Scheme);
            m_mailmanager.SendEmailTicketUpdate(Input.TicketRequestor, callbackUrl, Id);
        }

        //public void GetUsersInCompany(string userId)
        //{
        //    var CompanyFromUser = m_userInCompanyService.GetUsersInCompany().FirstOrDefault(x => x.UserId == userId);
        //}
        public Companies GetCompanyDetails(string userId)
        {
            var CompanyFromUser = m_userInCompanyService.GetUsersInCompany().FirstOrDefault(x => x.UserId == userId);
            var company = m_context.Company.Include(x => x.Contacten).FirstOrDefault(x => x.Id == CompanyFromUser.CompanyId);
            return company;
        }

    }
}