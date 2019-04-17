using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystem.Models
{

    public class ExactDivision
    {
        public D d { get; set; }
    }

    public class D
    {
        public Result[] results { get; set; }
    }

    public class Result
    {
        public __Metadata __metadata { get; set; }
        public int CurrentDivision { get; set; }
    }

    public class __Metadata
    {
        public string uri { get; set; }
        public string type { get; set; }
    }

}
