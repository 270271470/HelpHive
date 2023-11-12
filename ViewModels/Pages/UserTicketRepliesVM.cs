using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpHive.Models;
using HelpHive.Services;

namespace HelpHive.ViewModels.Pages
{
    public class UserTicketRepliesVM : ViewModelBaseClass
    {
        private readonly IDataAccessService _dataAccess;
        private readonly IUserService _userService;
        private UserModel _loggedInUser;
        private TicketModel _currentTicket;

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

        // Bindable property for the current ticket
        public TicketModel CurrentTicket
        {
            get { return _currentTicket; }
            set
            {
                _currentTicket = value;
                OnPropertyChanged(nameof(CurrentTicket)); // Notify view of the property change
            }
        }

        // Constructor
        public UserTicketRepliesVM(IDataAccessService dataAccess, IUserService userService)
        {
            _dataAccess = dataAccess;
            _userService = userService;
            //UserTicketReplies = new ObservableCollection<TicketReplies>(); //NB!
            LoadUserDetails();
            //LoadTicketReplies();
        }

        // Method to load user details
        public void LoadUserDetails()
        {
            if (_userService.CurrentUser != null)
            {
                LoggedInUser = _dataAccess.GetUserDetails(_userService.CurrentUser.Email);
            }
        }

        // Method to load ticket details. Call this method from the view's code-behind
        public void LoadTicketDetails(string ticketId)
        {
            // Replace with actual method to retrieve ticket details by ID
            // This is just a placeholder for illustration
            CurrentTicket = _dataAccess.GetTicketDetails(ticketId);
        }

    }
}
