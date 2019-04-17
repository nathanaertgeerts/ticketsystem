using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystem.Models
{
    public class APIKey
    {
        [Key]
        public int Id { get; set; }

        public string VSTStoken { get; set; }

        [EnumDataType(typeof(Source))]
        public Source Source { get; set; }

        public DateTime Date { get; set; }
    }

    public enum Source
    {
        VSTS = 1
    }
}
