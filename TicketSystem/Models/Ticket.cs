using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystem.Models
{
    public class Ticket
    {
        [Required]
        public int TicketId { get; set; }
        [Required(ErrorMessage = "Subject required")]
        public string TicketSubject { get; set; }
        [Required(ErrorMessage = "Details required")]
        public string TicketDetails { get; set; }
        [Required]
        public string TicketRequestor { get; set; }

        public DateTime TicketDate { get; set; }

        //status en priority vervangen door enum***
        [EnumDataType(typeof(Priority))]
        public Priority PriorityType { get; set; }
        [EnumDataType(typeof(Status))]
        public Status StatusType { get; set; }
        [EnumDataType(typeof(Product))]
        public Product ProductType { get; set; }
        public List<Reply> Replies { get; set; }

        public List<Bug> Bugs { get; set; }
        public Categories Category { get; set; }
    }
    public enum Priority
    {
        Normal = 1,
        Low = 2,
        High = 3
    }
    public enum Status
    {
        Open = 1,
        Pending = 2,
        Closed = 3
    }

    public enum Product
    {
        InControl = 1,
        InWave = 2,
        InDocumentation = 3,
        Overige = 4
    }
}
