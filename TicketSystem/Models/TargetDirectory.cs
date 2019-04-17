using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystem.Models
{
    public class TargetDirectory
    {
        public int Id { get; set; }
        public string DirectoryPath { get; set; }
        public List<DMS> FilePaths { get; set; }
    }
}
