using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketSystem.Data;
using TicketSystem.Services;

namespace TicketSystem.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminChangeRolesModel : PageModel
    {
        public class InputModel
        {
            public string Role { get; set; }
            public string id { get; set; }
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public List<ApplicationUser> Customers { get; set; }
        public List<ApplicationUser> Agents { get; set; }
        public List<ApplicationUser> Admins { get; set; }

        private ApplicationDbContext m_context;
        private UserManager<ApplicationUser> m_userManager;
        private SignInManager<ApplicationUser> m_signInManager;
        private RoleManager<IdentityRole> m_roleManager;
        private UserService m_userService;
        public AdminChangeRolesModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, UserService userService, SignInManager<ApplicationUser> signInManager)
        {
            m_context = context;
            m_userManager = userManager;
            m_roleManager = roleManager;
            m_userService = userService;
            m_signInManager = signInManager;
        }
        public void OnGet()
        {
            Customers = m_userManager.GetUsersInRoleAsync("Customer").Result.ToList();
            Agents = m_userManager.GetUsersInRoleAsync("Agent").Result.ToList();
            Admins = m_userManager.GetUsersInRoleAsync("Admin").Result.ToList();
        }
        //Delete User
        public IActionResult OnPost(string id)
        {
            var user = m_userManager.FindByIdAsync(id).Result;
            //first update security stamp van de gebruiker die we verwijderen
            m_userManager.UpdateSecurityStampAsync(user);
            m_userService.DeleteUser(id);

            
            // Clear the existing external cookie to ensure a clean login process
            //HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return RedirectToPage("../Admin/AdminChangeRoles");

        }
    }
}