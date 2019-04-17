using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketSystem.Services;

namespace TicketSystem.Pages.Knowledge
{
    [Authorize(Roles = "Admin, Agent")]
    public class ManageCategoriesModel : PageModel
    {

        public class InputModel
        {
            public int Id   { get; set; }
            public string Title { get; set; }

        }

        [BindProperty]
        public InputModel Input { get; set; }

        

        public Models.KnowledgeMap Map { get; set; }
        public List<Models.KnowledgeMap> MapAndArticles { get; set; }

        private KnowledgeService m_knowledgeService;
        public ManageCategoriesModel(KnowledgeService knowledgeService)
        {
            m_knowledgeService = knowledgeService;
        }
        public void OnGet(int id)
        {
            Map = m_knowledgeService.GetMapByID(id);
            MapAndArticles = m_knowledgeService.GetMapsAndArticles();

        }

        public IActionResult OnPostUpdateMap()
        {
            var Category = m_knowledgeService.UpdateMap(Input.Id, Input.Title);
            return RedirectToPage("../Knowledge/ManageKnowledge");
        }

        public IActionResult OnPostDelete()
        {
            m_knowledgeService.DeleteMap(Input.Id);
            return RedirectToPage("../Knowledge/ManageKnowledge");
        }

    }
}