using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystem.Models
{
    public class UserInCompany
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CompanyId { get; set; }
    }
}
