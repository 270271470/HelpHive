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
        bool RegisterUser(UserModel user);
    }
}