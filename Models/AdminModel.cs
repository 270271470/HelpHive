using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpHive.Models
{
    // the AdminModel maps to tbladmins in our remote database
    public class AdminModel
    {
        public int AdminId { get; set; }             // aid is auto-incremented by database
        public int RoleId { get; set; }              // foreignkey from tbladminroles
        public string UserName { get; set; }
        public string Password { get; set; }        // Storing a hashed password, not plain text
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";    // combining the fname and lname into fullname for convenience
        public string Email { get; set; }
        public string Departments { get; set; }
        public string TicketNotifications { get; set; }
        public DateTime DateCreated { get; set; }   // DateCreated set to  current date/time when creating a new admin
    }
}