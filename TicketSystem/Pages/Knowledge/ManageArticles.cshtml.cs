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

namespace TicketSystem.Pages.Knowledge
{
    [Authorize(Roles = "Admin, Agent")]
    public class ManageArticlesModel : PageModel
    {
        public class InputModel
        {
            public string Title { get; set; }
            public string Content { get; set; }
            public int Id { get; set; }
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public List<Models.Article> Articles { get; set; }

        public List<Models.KnowledgeMap> Categories { get; set; }

        public Models.KnowledgeMap CurrentCategory { get; set; }

        private KnowledgeService m_knowledgeService;
        private IMailManager m_mailManager;
        private ApplicationDbContext m_context;
        private UserManager<ApplicationUser> m_userManager;

        public ManageArticlesModel(KnowledgeService knowledgeService, IMailManager mailManager, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            m_knowledgeService = knowledgeService;
            m_mailManager = mailManager;
            m_context = context;
            m_userManager = userManager;

        }
        public void OnGet()
        {
            Articles = m_knowledgeService.GetArticles();
            Categories = m_knowledgeService.GetMaps();
        }

        public IActionResult OnPostCreateArticle()
        {
            m_knowledgeService.CreateArticle(Input.Title, Input.Content, Input.Id);
            var link = Url.Knowledgebase(Request.Scheme);

            var Customers = m_userManager.GetUsersInRoleAsync("Customer").Result.ToList();

            foreach (var Customer in Customers)
            {
                m_mailManager.SendEmailNewArticle(Customer.Email, link);
            }
           


            return RedirectToPage("../Knowledge/ManageArticles");
        }
    }
}