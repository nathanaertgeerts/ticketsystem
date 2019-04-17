using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TicketSystem.Data;
using TicketSystem.Services;

namespace TicketSystem.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IMailManager m_mailmanager;
        private ApplicationDbContext m_context;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<LoginModel> logger,
            IMailManager mailmanager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            m_mailmanager = mailmanager;
            m_context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public string Company { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, Company = Input.Company, Firstname = Input.Firstname, Lastname = Input.Lastname };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    //add user to role
                    var test2 = _userManager.AddToRoleAsync(user, "Customer").Result;
                    //add notifcation settings
                    var userNotifications = _userManager.Users.Include(x => x.Notifications).FirstOrDefault(x => x.UserName == user.UserName);
                    userNotifications.Notifications.Add(new Models.Notifications()
                    {
                        TicketCreated = true,
                        TicketUpdate = true,
                        NewArticle = true,
                        NewDocument = true
                    });
                    m_context.SaveChanges();

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                    m_mailmanager.SendEmailConfirmationAsync(Input.Email, callbackUrl, Input.Email);

                    //Prevent newly registered users from being automatically logged on
                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToPage("./RegisterSucces" ,new { id = Input.Email } );
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    if (error.Code == "DuplicateUserName")
                    {
                        return RedirectToPage("./ErrorHandling");
                    }
                }
                
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
