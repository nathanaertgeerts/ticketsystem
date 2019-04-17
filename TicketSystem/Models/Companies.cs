using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketSystem.Data;

namespace TicketSystem.Models
{
    public class Companies
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Postalcode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Homepage { get; set; }
        public ContractLevel ContractLevel { get; set; }
        public List<Contacts> Contacten { get; set; }
    }

    public enum ContractLevel
    {
        None = 1,
        Bronze = 2,
        Silver = 3,
        Gold = 4
    }
    
}
