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
        void UpdateTicketRating(TicketReplyModel reply);
        void UpdateTicketStatus(TicketModel ticket);
        UserModel VerifyUser(string email, string hashedPassword);
        UserModel GetUserDetails(string email);
        AdminModel GetAdminDetails(string email);
        AdminModel VerifyAdmin(string email, string hashedPassword);
        List<TicketDeptsModel> GetDepartments();
        List<AdminModel> GetAdmins();
        List<AdminRolesModel> GetAdminRoles();
        List<TicketModel> GetUserOpenTickets(int userId);
        List<TicketModel> GetOpenTicketsAsAdmin();
        List<TicketModel> GetAdminTicketHistory();
        List<TicketModel> GetUserTicketHistory(int userId);
        List<TicketReplyModel> GetTicketReplies(string ticketId);
        TicketModel GetTicketDetails(string ticketId);
        bool AdminOriginalUpdateTicket(TicketModel ticket);
        bool UserOriginalUpdateTicket(TicketModel ticket);
        bool RegisterUser(UserModel user);
        bool CreateNewTicket(TicketModel ticket);
        bool InsertUserTicketReply(TicketReplyModel ticketreply);
        bool InsertAdminTicketReply(TicketReplyModel ticketreply);
    }
}