using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpHive.Models
{
    public class TicketReplyModel
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

        // Rename this property to match XAML binding
        public DateTime PostedDate
        {
            get { return Date; }
            set { Date = value; }
        }

        // Used to display who posted the reply in the UI
        public string PostedBy
        {
            get { return string.IsNullOrEmpty(Admin) ? Name : Admin; }
        }

        // Property to indicate if the reply is from an admin for UI logic
        public bool IsAdminReply
        {
            get { return !string.IsNullOrEmpty(Admin); }
        }
    }

}