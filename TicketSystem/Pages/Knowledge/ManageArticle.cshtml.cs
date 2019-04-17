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
    public class ManageArticleModel : PageModel
    {
        public class InputModel
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }

            public int CategoryID { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public Models.Article Article { get; set; }

        public Models.KnowledgeMap Category { get; set; }
        public List<Models.KnowledgeMap> AllCategories { get; set; }


        private KnowledgeService m_knowledgeService;
        public ManageArticleModel(KnowledgeService knowledgeService)
        {
            m_knowledgeService = knowledgeService;
        }
        public void OnGet(int id)
        {
            Article = m_knowledgeService.GetArticleById(id);
            AllCategories = m_knowledgeService.GetMaps();
            foreach (var item in AllCategories)
            {
                if (item.Articles == null)
                {
                    //no articles
                }
                else
                {
                    foreach (var Article in item.Articles)
                    {
                        if (Article.Id == id)
                        {
                            Category = m_knowledgeService.GetMapByID(item.Id);
                        }
                    }
                }
                
            }

        }

        public IActionResult OnPostUpdateArticle()
        {
            m_knowledgeService.UpdateArticle(Input.Id, Input.Title, Input.Content, Input.CategoryID);
            return RedirectToPage("../Knowledge/ManageArticles");
        }

        public IActionResult OnPostDelete()
        {
            m_knowledgeService.DeleteArticle(Input.Id);
            return RedirectToPage("../Knowledge/ManageKnowledge");
        }
    }
}