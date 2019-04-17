using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketSystem.Data;
using TicketSystem.Services;

namespace TicketSystem.Pages.Knowledge
{
    [Authorize(Roles = "Admin, Agent")]
    public class ManageKnowledgeModel : PageModel
    {
        public class InputModel
        {
            public string Title { get; set; }
        }
        [BindProperty]
        public InputModel Input { get; set; }

        public List<Models.KnowledgeMap> Maps { get; set; }
        

        private KnowledgeService m_knowledgeService;

        public ManageKnowledgeModel(KnowledgeService knowledgeService)
        {
            m_knowledgeService = knowledgeService;
        }
        public void OnGet()
        {
            Maps = m_knowledgeService.GetMapsAndArticles();
        }

        public IActionResult OnPostMap()
        {
            m_knowledgeService.CreateMap(Input.Title);
            Maps = m_knowledgeService.GetMapsAndArticles();

            return RedirectToPage("../Knowledge/ManageKnowledge");
        }
    }
}