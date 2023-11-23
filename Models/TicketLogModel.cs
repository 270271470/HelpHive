using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpHive.Models
{
    // the class maps to the tblticketlogs where we store our system logs.
    class TicketLogModel
    {
        public DateTime Date { get; set; }
        public string TicketId { get; set; }
        public string Action { get; set; }

    }
}
