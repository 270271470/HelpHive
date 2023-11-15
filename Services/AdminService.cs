using HelpHive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpHive.Services
{
    class AdminService : IAdminService
    {
        public AdminModel CurrentAdmin { get; private set; }

        public void Login(AdminModel admin)
        {
            CurrentAdmin = admin; // Set admin as logged-in
        }

        public void Logout()
        {
            CurrentAdmin = null; // Clear admin on logout
        }
    }
}
