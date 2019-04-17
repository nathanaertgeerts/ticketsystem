using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystem.Models
{
    public class MailTicket
    {
        public int MailTicketId { get; set; }
        public string Requestor { get; set; }
        public string Subject   { get; set; }
        public DateTime Date { get; set; }
        public string Body { get; set; }
    }
}
