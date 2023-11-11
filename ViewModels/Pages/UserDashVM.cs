using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpHive.Commands;
using HelpHive.Models;
using HelpHive.Services;
using HelpHive.DataAccess;
using System.Collections.ObjectModel;

namespace HelpHive.ViewModels.Pages
{
    public class UserDashVM : ViewModelBaseClass
    {
        private readonly IDataAccessService _dataAccess;
        private readonly IUserService _userService;
        private UserModel _loggedInUser;
        public ObservableCollection<TicketModel> UserOpenTickets { get; private set; }

        // Bindable property for the View
        public UserModel LoggedInUser
        {
            get { return _loggedInUser; }
            set
            {
                _loggedInUser = value;
                OnPropertyChanged(nameof(LoggedInUser)); // Notify view of the property change
            }
        }

        // Constructor
        public UserDashVM(IDataAccessService dataAccess, IUserService userService)
        {
            _dataAccess = dataAccess;
            _userService = userService;
            UserOpenTickets = new ObservableCollection<TicketModel>(); //NB!
            LoadUserDetails();
            LoadOpenTickets();
        }

        // Method to load user details
        public void LoadUserDetails()
        {
            if (_userService.CurrentUser != null)
            {
                LoggedInUser = _dataAccess.GetUserDetails(_userService.CurrentUser.Email);
            }
        }
        // Method to load tickets
        private void LoadOpenTickets()
        {
            var uid = LoggedInUser.UserId;
            var tickets = _dataAccess.GetUserOpenTickets(uid);

            UserOpenTickets.Clear(); // Clear existing collection
            foreach (var ticket in tickets)
            {
                UserOpenTickets.Add(ticket); // Add items to existing collection
            }
        }

    }

}