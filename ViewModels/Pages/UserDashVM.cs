using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpHive.Commands;
using HelpHive.Models;
using HelpHive.Services;
using HelpHive.DataAccess;

namespace HelpHive.ViewModels.Pages
{
    public class UserDashVM : ViewModelBaseClass // Assuming you inherit from a base ViewModel class
    {
        //private readonly DataAccessLayer _dataAccessLayer;
        private readonly IDataAccessService _dataAccess;
        private readonly IUserService _userService;
        private UserModel _loggedInUser;

        // Bindable property for the View
        public UserModel LoggedInUser
        {
            get { return _loggedInUser; }
            set
            {
                _loggedInUser = value;
                OnPropertyChanged(nameof(LoggedInUser)); // Notify the view of the property change
            }
        }

        // Constructor
        public UserDashVM(IDataAccessService dataAccess, IUserService userService)
        {
            _dataAccess = dataAccess;
            _userService = userService;
            LoadUserDetails();
        }

        // Method to load user details
        public void LoadUserDetails()
        {
            if (_userService.CurrentUser != null)
            {
                LoggedInUser = _dataAccess.GetUserDetails(_userService.CurrentUser.Email);
            }
        }

    }

}
