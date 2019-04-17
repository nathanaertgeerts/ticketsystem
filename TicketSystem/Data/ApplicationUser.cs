using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TicketSystem.Models;

namespace TicketSystem.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public List<APIKey> ApiKeys {get; set;}

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Company { get; set; }

        public List<Notifications> Notifications { get; set; }
    }
}
