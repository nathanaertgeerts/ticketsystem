using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystem.Models
{
    public class Contacts
    {
        [Key]
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string BEmail { get; set; }
        public string BMPhone { get; set; }
        public string BPhone { get; set; }
        public string Phone { get; set; }
        public string MPhone { get; set; }
        public string JobTitle { get; set; }
    }
}
