using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystem.Models
{
    public class DMS
    {

        public int Id { get; set; }
        public string FilePath { get; set; }
        public DocumentLevel DocumentLevel { get; set; }
        
    }
    public enum DocumentLevel
    {
        None = 1,
        Bronze = 2,
        Silver = 3,
        Gold = 4,
        Invisible = 5 

    }
}
