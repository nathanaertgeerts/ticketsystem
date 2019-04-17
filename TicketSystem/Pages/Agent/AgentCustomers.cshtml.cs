using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketSystem.Data;
using TicketSystem.Services;

namespace TicketSystem.Pages.Agent
{
    [Authorize(Roles = "Admin, Agent")]
    public class AgentCustomersModel : PageModel
    {
        //Lijst van alle users
        public List<ApplicationUser> Users { get; set; }

        //iets van het object user
        public ApplicationUser Customer { get; set; }

        //User service binnenhalen
        private UserService m_userService;
        public AgentCustomersModel(UserService userService)
        {
            m_userService = userService;
        }

        //onGet alle roles binnenhalen
        public void OnGet(string roleName)
        {
            //in Customers roleName = customers
            var var = "Customer";

            Users = m_userService.GetUsersInRole(var);
        }

        //Delete roles
        public IActionResult OnPost(string id)
        {
            Customer = m_userService.DeleteUser(id);

            return RedirectToPage("../Admin/AdminRoles");

        }
    }
}