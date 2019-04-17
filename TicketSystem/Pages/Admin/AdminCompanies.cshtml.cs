using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketSystem.Models;
using TicketSystem.Services;

namespace TicketSystem.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminCompaniesModel : PageModel
    {
        private ExactOnlineService m_exactOnlineService;

        public List<Companies> Companies { get; set; }

        public AdminCompaniesModel(ExactOnlineService exactOnlineService)
        {
            m_exactOnlineService = exactOnlineService;
        }
        public void OnGet(string searchString)
        {
            Companies = m_exactOnlineService.GetCompanies();

            ViewData["CurrentFilter"] = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                Companies = Companies.Where(x => x.Name.ToLower().Contains(searchString.ToLower())).ToList();

                if (Companies.Count == 0)
                {
                    ModelState.AddModelError(string.Empty, "No matching results");
                }
            }
            
        }
    }
}