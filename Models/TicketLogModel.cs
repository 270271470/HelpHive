using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpHive.Models
{
    class TicketLogModel
    {
        public DateTime Date { get; set; }
        public string TicketId { get; set; }
        public string Action { get; set; }

    }
}
