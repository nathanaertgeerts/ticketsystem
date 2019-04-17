using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketSystem.Services;

namespace TicketSystem.Pages.Knowledge
{
    public class ArticleViewModel : PageModel
    {
        public Models.Article Article { get; set; }

        private KnowledgeService m_knowledgeService;
        public ArticleViewModel(KnowledgeService knowledgeService)
        {
            m_knowledgeService = knowledgeService;
        }
        public void OnGet(int id)
        {
            Article = m_knowledgeService.GetArticleById(id);
        }
    }
}