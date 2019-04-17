using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TicketSystem.Pages.Account
{
    public class RegisterSuccesModel : PageModel
    {
        public string Email { get; set; }
        public void OnGet(string Id)
        {
            Email = Id;
        }
    }
}