using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using HelpHive.Commands;
using HelpHive.Models;
using HelpHive.Services;

namespace HelpHive.ViewModels.Pages
{
    public class UserTicketRepliesVM : ViewModelBaseClass
    {
        private readonly IDataAccessService _dataAccess;
        private readonly IUserService _userService;
        private readonly ITicketService _ticketService;
        private readonly INavigationService _navigationService;
        private UserModel _loggedInUser;
        private TicketModel _currentTicket;
        private TicketReplyModel _ticketreply;

        public RelayCommand UpdateTicketCommand { get; private set; }
        public RelayCommand NavigateToUserDashCommand { get; private set; }

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
        public UserTicketRepliesVM(IDataAccessService dataAccess, IUserService userService, INavigationService navigationService)
        {
            _dataAccess = dataAccess;
            _userService = userService;
            _navigationService = navigationService;
            //_ticketService = ticketService;
            //UserTicketReplies = new ObservableCollection<TicketReplies>(); //NB!
            LoadUserDetails();
            //LoadTicketReplies();

            NavigateToUserDashCommand = new RelayCommand(ExecuteNavigateToUserDash);

            UpdateTicketCommand = new RelayCommand(UpdateTicket, CanUpdateTicket);

        }

        private bool CanUpdateTicket(object parameter)
        {
            // Updated validation logic
            return !string.IsNullOrWhiteSpace(UserMessage);
        }

        // Method to handle ticket update
        private void UpdateTicket(object parameter)
        {
            Debug.WriteLine("Create Ticket method called");
            try
            {
                var ticketReply = new TicketReplyModel
                {
                    Tid = CurrentTicket.TicketId,
                    UserId = LoggedInUser.UserId,
                    Name = LoggedInUser.FirstName + " " + LoggedInUser.LastName,
                    Email = LoggedInUser.Email,
                    Date = DateTime.Now,
                    Message = this.UserMessage,
                    // Set Rating if applicable
                };

                // Use the data access layer to save the new ticket
                var success = _dataAccess.InsertUserTicketReply(ticketReply);
                if (success)
                {
                    MessageBox.Show("New ticket reply");

                    _navigationService.NavigateTo("UserDash");

                }
                else
                {
                    MessageBox.Show("Ticket creation failed. Please check the entered information and try again.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while creating the ticket. Please try again later.");
                Debug.WriteLine($"Ticket creation failed: {ex.Message}");
            }
        }


        private void ExecuteNavigateToUserDash(object parameter)
        {
            _navigationService.NavigateTo("UserDash");
        }


        private string _userMessage;
        public string UserMessage
        {
            get { return _userMessage; }
            set
            {
                if (_userMessage != value)
                {
                    _userMessage = value;
                    OnPropertyChanged();
                }
            }
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
            // Method to retrieve CurrentTicket details by ID
            CurrentTicket = _dataAccess.GetTicketDetails(ticketId);
        }

    }
}