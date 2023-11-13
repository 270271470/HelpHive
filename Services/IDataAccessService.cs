using HelpHive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpHive.Services
{
    public interface IDataAccessService
    {
        UserModel VerifyUser(string email, string hashedPassword);
        UserModel GetUserDetails(string email);
        List<TicketDeptsModel> GetDepartments();
        List<TicketModel> GetUserOpenTickets(int userId);
        List<TicketReplyModel> GetTicketReplies(string ticketId);
        TicketModel GetTicketDetails(string ticketId);
        bool RegisterUser(UserModel user);
        bool CreateNewTicket(TicketModel ticket);
        bool InsertUserTicketReply(TicketReplyModel ticketreply);
        bool InsertAdminTicketReply(TicketReplyModel ticketreply);
    }
}