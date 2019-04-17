using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketSystem.Services;
using Microsoft.AspNetCore.Identity;
using TicketSystem.Data;

namespace TicketSystem.Pages.Private
{
    public class PrivatePageModel : PageModel
    {
        public string Message { get; private set; }

        public List<Models.KnowledgeMap> Maps { get; set; }
        public List<Models.Article> Articles { get; set; }


        private KnowledgeService m_knowledgeService;
        private UserManager<ApplicationUser> m_userManager;
        private SignInManager<ApplicationUser> m_signInManager;

        public PrivatePageModel(KnowledgeService knowledgeService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            m_knowledgeService = knowledgeService;
            m_userManager = userManager;
            m_signInManager = signInManager;
        }
        public void OnGet(string searchString)
        {
            //var user2 = m_userManager.FindByNameAsync(User.Identity.Name).Result;
            //m_signInManager.RefreshSignInAsync(user2);

            Maps = m_knowledgeService.GetMaps();
            Articles = m_knowledgeService.GetArticles();

            ViewData["CurrentFilter"] = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                Maps = Maps.Where(x => x.Title.ToLower().Contains(searchString.ToLower())).ToList();

                Articles = Articles.Where(x => x.Content.ToLower().Contains(searchString.ToLower())).ToList();

                if (Maps.Count == 0)
                {
                    ModelState.AddModelError(string.Empty, "No matching results");
                }
            }
        }
    }
}