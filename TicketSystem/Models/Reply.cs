using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystem.Models
{
    public class Reply
    {
        [Key]
        public int ReplyId { get; set; }
        [Required]
        public string ReplyRequestor { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime ReplyDate { get; set; }

    }
}
