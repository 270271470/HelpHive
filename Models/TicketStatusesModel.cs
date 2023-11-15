using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpHive.Models
{
    class TicketStatusesModel
    {
        //Creating the following for 
        //The ticket Status. 
        public string Title { get; set; }
        public string Color { get; set; }
        public int SortOrder { get; set; }
        public int ShowActive { get; set; }
        public int ShowAwaiting { get; set; }
        public int AutoClose { get; set; }
    }
}
