using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpHive.Models
{
    public class TicketModel
    {
        public string TicketId { get; set; }
        public int DeptId { get; set; }
        public string DepartmentName { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string TicketStatus { get; set; }
        public string IncidentStatus { get; set; }
        public string Urgency { get; set; }
        public string Admin { get; set; }
        public string Attachment { get; set; }
        public string AttachmentsRemoved { get; set; }
        public DateTime LastReply { get; set; }
        public int ClientUnread { get; set; }
        public string AdminRead { get; set; }
        public DateTime ReplyTime { get; set; }

        public bool MatchesSearch(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return true;
            }

            // Check if search text is in the TicketId, Title, or Message.
            return (TicketId?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                   (Title?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                   (DepartmentName?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                   (Name?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                   (Message?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0);
        }

    }
}