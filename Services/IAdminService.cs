using HelpHive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpHive.Services
{
    public interface IAdminService
    {
        AdminModel CurrentAdmin { get; }
        void Login(AdminModel user);
        void Logout();
    }
}
