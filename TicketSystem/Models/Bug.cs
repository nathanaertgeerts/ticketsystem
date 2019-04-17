using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystem.Models
{
    public class Bug
    {
        [Key]
        public int BugId { get; set; }

        [JsonProperty("id")]
        public string VstsBugId { get; set; }

        public string State { get; set; }

        public string AssignedTo { get; set; }

        public string ProjectName { get; set; }

        public string Subject { get; set; }

    }
}
