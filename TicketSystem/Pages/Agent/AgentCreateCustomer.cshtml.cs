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
using TicketSystem.Services;

namespace TicketSystem.Pages.Agent
{
    [Authorize(Roles = "Admin, Agent")]
    public class AgentCreateCustomerModel : PageModel
    {
        public class InputModel
        {
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }
            public string Password { get; set; }

            public string Firstname { get; set; }
            public string Lastname { get; set; }

        }

        [BindProperty]
        public InputModel Input { get; set; }

        //user service binnenhalen
        private UserService m_userService;
        private UserManager<ApplicationUser> m_userManager;
        private ApplicationDbContext m_context;
        //Email sender interface
        private readonly IMailManager m_mailmanager;

        public AgentCreateCustomerModel(UserService userService, UserManager<ApplicationUser> userManager, IMailManager mailManager, ApplicationDbContext context)
        {
            m_userService = userService;
            m_userManager = userManager;
            m_mailmanager = mailManager;
            m_context = context;
        }

        //interface actie - resultaat onPost (submit roept die aan)
        public IActionResult OnPost()
        {

            var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, Firstname = Input.Firstname, Lastname = Input.Lastname };
            var result = m_userManager.CreateAsync(user).Result;
            if (result.Succeeded)
            {
                //add user to role
                var test2 = m_userManager.AddToRoleAsync(user, Input.Role).Result;
                //new account has been created
                //add notifcation settings
                var userNotifications = m_userManager.Users.Include(x => x.Notifications).FirstOrDefault(x => x.UserName == user.UserName);
                userNotifications.Notifications.Add(new Models.Notifications()
                {
                    TicketCreated = true,
                    TicketUpdate = true,
                    NewArticle = true,
                    NewDocument = true
                });
                m_context.SaveChanges();

                //stuur een Email confirmatie mail  ....
                var code = m_userManager.GenerateEmailConfirmationTokenAsync(user).Result;
                var callbackUrl = Url.EmailConfirmationNewTicketLink(user.Id, code, Request.Scheme);
                m_mailmanager.SendEmailConfirmationAsync(Input.Email, callbackUrl, Input.Email);

                // zorg ervoor dat de gebruiker hier ook zijn passwoord kan setten
            }
            else
            {
                return RedirectToPage("../Error");
            }

            return RedirectToPage("../Agent/AgentCustomers");
        }
    }
}