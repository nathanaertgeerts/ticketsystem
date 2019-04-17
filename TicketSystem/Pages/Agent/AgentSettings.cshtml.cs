using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketSystem.Models;
using TicketSystem.Services;

namespace TicketSystem.Pages.Agent
{
    [Authorize(Roles = "Admin, Agent")]
    public class AgentSettingsModel : PageModel
    {
        public class InputModel
        {
            public string Key { get; set; }
            public int id { get; set; }
        }
        [BindProperty]
        public InputModel Input { get; set; }

        public List<APIKey> ApiKeys { get; set; }

        private UserService m_userService;
        //Email service binnenhalen
        public AgentSettingsModel(UserService userService)
        {
            m_userService = userService;
        }
        public void OnGet()
        {
            ApiKeys = m_userService.GetApiKeys(User.Identity.Name);
        }

        public IActionResult OnPostSetVSTSToken()
        {
            m_userService.AddAPIKey(Models.Source.VSTS, User.Identity.Name, Input.Key, DateTime.Now);
            // userService ophalen en de functie Add VSTS token aanroepen
            return RedirectToPage("../Agent/AgentSettings");
        }

        public IActionResult OnPostDeleteToken(int id)
        {
            m_userService.DeleteAPIKey(Input.id);
            return RedirectToPage("../Agent/AgentSettings");
        }
    }
}