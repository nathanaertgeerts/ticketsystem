using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystem.Models
{
    public class BugObject
    {
        public int count { get; set; }
        public ValueObject[] value { get; set; }

    }
    public class ValueObject
    {
        public int id { get; set; }
        public int rev { get; set; }
        public Fields fields { get; set; }
        public string url { get; set; }
    }

    public class Fields
    {
        [JsonProperty("System.State")]
        public string State { get; set; }
        [JsonProperty("System.AssignedTo")]
        public string AssignedTo { get; set; }
        [JsonProperty("System.AreaPath")]
        public string ProjectName { get; set; }

        [JsonProperty("System.Title")]
        public string Subject { get; set; }

    }
}
