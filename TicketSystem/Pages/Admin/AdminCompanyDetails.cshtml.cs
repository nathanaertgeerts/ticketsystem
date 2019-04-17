using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketSystem.Data;
using TicketSystem.Models;
using TicketSystem.Services;

namespace TicketSystem.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminCompanyDetailsModel : PageModel
    {
        public class InputModel
        {
            public Models.ContractLevel ContractLevel { get; set; }

        }

        [BindProperty]
        public InputModel Input { get; set; }

        private ExactOnlineService m_exactOnlineService;
        private UserInCompanyService m_userInCompanyService;
        private ApplicationDbContext m_context;
        private UserManager<ApplicationUser> m_userManager;

        public Companies Companies { get; set; }

        public List<UserInCompany> UserInCompany {get; set;}
        public List<ApplicationUser> UsersList { get; set; }

        public AdminCompanyDetailsModel(ExactOnlineService exactOnlineService, UserInCompanyService userInCompanyService, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            m_exactOnlineService = exactOnlineService;
            m_userInCompanyService = userInCompanyService;
            m_context = context;
            m_userManager = userManager;
            this.UsersList = new List<ApplicationUser>();
        }
        public void OnGet(int id)
        {
            Companies = m_exactOnlineService.GetCompanyById(id);

            UsersInCompany(id);
        }

        public List<ApplicationUser> UsersInCompany(int id)
        {
            UserInCompany = m_userInCompanyService.GetUsersInCompany().FindAll(x => x.CompanyId == id).ToList();

            foreach (var item in UserInCompany)
            {
                var Gebruiker = m_userManager.FindByIdAsync(item.UserId).Result;
                UsersList.Add(Gebruiker);
            }

            return UsersList;
        }

        public IActionResult OnPostUpdateCompany(int id)
        {

            var updateCompany = m_context.Company.FirstOrDefault(x => x.Id == id);

            updateCompany.ContractLevel = Input.ContractLevel;
            m_context.SaveChanges();

            return RedirectToPage("../Admin/AdminCompanies");
        }
    }
}