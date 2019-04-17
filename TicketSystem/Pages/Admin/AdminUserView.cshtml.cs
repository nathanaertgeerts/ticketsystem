using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Data;
using TicketSystem.Models;
using TicketSystem.Services;

namespace TicketSystem.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminUserViewModel : PageModel
    {
        public class InputModel
        {
            public string Role { get; set; }
            public string id { get; set; }
            public string email { get; set; }
            public int CompanyID { get; set; }
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public ApplicationUser UserDetails { get; set; }
        public Models.Companies Company { get; set; } 
        public List<Companies> Companies { get; set; }
        public string CurrentRole { get; set; }
        public Models.UserInCompany UserInCompany { get; set; }


        private ApplicationDbContext m_context;
        private UserManager<ApplicationUser> m_userManager;
        private RoleManager<IdentityRole> m_roleManager;
        private UserService m_userService;
        private ExactOnlineService m_exactOnlineService;
        private UserInCompanyService m_userInCompanyService;
        public AdminUserViewModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, UserService userService, ExactOnlineService exactOnlineService, UserInCompanyService userInCompanyService)
        {
            m_context = context;
            m_userManager = userManager;
            m_roleManager = roleManager;
            m_userService = userService;
            m_exactOnlineService = exactOnlineService;
            m_userInCompanyService = userInCompanyService;
        }
        public IActionResult OnGet(string id)
        {
            UserDetails = m_userManager.FindByIdAsync(id).Result;
            var lijst = m_userManager.GetRolesAsync(UserDetails).Result;

            CurrentRole = lijst.FirstOrDefault();

            Companies = m_exactOnlineService.GetCompanies();

            UserInCompany = m_userInCompanyService.GetUsersInCompany().Find(x => x.UserId == UserDetails.Id);

            return Page();
        }
        public IActionResult OnPostSearch(string searchString)
        {
            UserDetails = m_userManager.FindByIdAsync(Input.id).Result;
            var lijst = m_userManager.GetRolesAsync(UserDetails).Result;

            CurrentRole = lijst.FirstOrDefault();

            Companies = m_exactOnlineService.GetCompanies();

            UserInCompany = m_userInCompanyService.GetUsersInCompany().Find(x => x.UserId == UserDetails.Id);

            ViewData["CurrentFilter"] = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                Companies = Companies.Where(x => x.Name.ToLower().Contains(searchString.ToLower())).ToList();

                if (Companies.Count == 0)
                {
                    ModelState.AddModelError(string.Empty, "No matching results");
                }
            }
            return Page();
        }

        public IActionResult OnPostRemoveRole()
        {
            m_userService.RemoveUserFromRole(Input.id, Input.Role);

            return RedirectToPage("../Admin/AdminChangeRoles");
        }

        public IActionResult OnPostAddUserToCompany(int id)
        {
            UserDetails = m_userManager.FindByEmailAsync(Input.email).Result;

            var test = m_userInCompanyService.GetUsersInCompany().Find(x => x.UserId == UserDetails.Id);
            if (test == null)
            {
                m_userInCompanyService.AddUserToCompany(UserDetails.Id, id);
                return RedirectToPage("../Admin/AdminChangeRoles");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User is already in a company!");

                Companies = m_exactOnlineService.GetCompanies();
                UserDetails = m_userManager.FindByIdAsync(Input.id).Result;
                UserInCompany = m_userInCompanyService.GetUsersInCompany().Find(x => x.UserId == UserDetails.Id);
                var lijst = m_userManager.GetRolesAsync(UserDetails).Result;
                CurrentRole = lijst.FirstOrDefault();

                return Page();
            }

        }

        public IActionResult OnPostDeleteUserFromCompany()
        {
            var test = m_context.UserInCompany.FirstOrDefault(x => x.Id == Input.CompanyID);
            m_context.Remove(test);
            m_context.SaveChanges();

            UserDetails = m_userManager.FindByIdAsync(Input.id).Result;
            Companies = m_exactOnlineService.GetCompanies();
            return Page();
        }
    }
}