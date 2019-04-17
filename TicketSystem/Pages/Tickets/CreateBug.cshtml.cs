using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using TicketSystem.Models;
using TicketSystem.Services;
using Microsoft.AspNetCore.Identity;
using TicketSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace TicketSystem.Pages.Tickets
{
    public class CreateBugModel : PageModel
    {
        public class InputModel
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public int TicketId { get; set; }
            public string Project { get; set; }
            public string File { get; set; }
        }
        [BindProperty]
        public InputModel Input { get; set; }

        //lijst van het model value
        public List<Models.Value> Value { get; set; }
        public Models.Ticket Ticket { get; set; }

        //Ticket service binnenhalen
        private TicketService m_ticketService;
        private VSTSService m_VSTSService;
        private UserManager<ApplicationUser> m_userManager;

        public CreateBugModel(TicketService ticketService, VSTSService vstsService, UserManager<ApplicationUser> userManager)
        {
            m_ticketService = ticketService;
            m_VSTSService = vstsService;
            m_userManager = userManager;
        }

        public IActionResult OnGet(int id)
        {
            var user = m_userManager.Users.Include(x => x.ApiKeys).FirstOrDefault(x => x.UserName == User.Identity.Name).ApiKeys.ToList();
            foreach (var item in user)
            {
                if (item.Source == Source.VSTS)
                {
                    var token = item.VSTStoken;

                    Value = m_VSTSService.GetProjects(token);
                }
            }
                    

            Ticket = m_ticketService.GetTicketById(id);
            Input = new InputModel()
            {
                TicketId = id
            };

            if (Ticket == null)
                return RedirectToPage("../Tickets/MyTickets");

            return Page();

        }

        //public void GetProjectList(string token)
        //{
        //    var projectlist = m_VSTSService.GetProjects(token);

        //}
        public IActionResult OnPostCreateBug()
        {
            var description = Input.Description + " " + Url.TicketLinkID(Request.Scheme);
            m_VSTSService.CreateBug(User.Identity.Name, Input.Title, description, Input.TicketId, Input.Project);

            return RedirectToPage("../Tickets/MyTickets");
        }
        public IActionResult OnPostCreateFeature()
        {
            var description = Input.Description + " " + Url.TicketLinkID(Request.Scheme);
            m_VSTSService.CreateFeature(User.Identity.Name, Input.Title, description, Input.TicketId, Input.Project);

            return RedirectToPage("../Tickets/MyTickets");
        }

    }
}