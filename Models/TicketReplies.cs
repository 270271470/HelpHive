using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpHive.Models
{
    public class TicketReplies
    {
        public string Tid { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public string Admin { get; set; }
        public string Attachment { get; set; }
        public byte AttachmentsRemoved { get; set; }
        public int? Rating { get; set; }
    }
}
