using HelpHive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpHive.Services
{
    public interface IUserService
    {
        //User Services
        //for log-in. 
        UserModel CurrentUser { get; }
        void Login(UserModel user);
        void Logout();
    }
}