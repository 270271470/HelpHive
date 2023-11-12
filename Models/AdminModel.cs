using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpHive.Models
{
    public class AdminModel
    {
        public int UserId { get; set; }             // uid is auto-incremented by database
        public string FirstName { get; set; }
        public string LastName { get; set; }        // CompanyName is optional, can be null
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }        // Storing a hashed password, not plain text
        public string Address1 { get; set; }
        public string Address2 { get; set; }        // Address2 is optional, can be null
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string Status { get; set; }          // Status set when creating a new user to "Active"
        public DateTime DateCreated { get; set; }   // DateCreated set to  current date/time when creating a new user
    }
}
