using HelpHive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpHive.Services
{
    // the service for users  - keeps track of legged in users and when they log out
    public class UserService : IUserService
    {
        public UserModel CurrentUser { get; private set; }

        public void Login(UserModel user)
        {
            CurrentUser = user; // Set user as logged-in
        }

        public void Logout()
        {
            CurrentUser = null; // Clear user on logout
        }
    }
}