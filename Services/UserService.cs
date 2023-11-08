using HelpHive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpHive.Services
{
    public class UserService : IUserService
    {
        public UserModel CurrentUser { get; private set; }

        public void Login(UserModel user)
        {
            CurrentUser = user; // Set the user as logged-in
        }

        public void Logout()
        {
            CurrentUser = null; // Clear the user on logout
        }
    }
}