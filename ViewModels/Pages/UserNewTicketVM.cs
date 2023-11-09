using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpHive.Commands;
using HelpHive.Models;
using HelpHive.Services;
using HelpHive.DataAccess;
using System.Diagnostics;
using System.Windows;

namespace HelpHive.ViewModels.Pages
{
    public class UserNewTicketVM : ViewModelBaseClass
    {
        //private readonly DataAccessLayer _dataAccessLayer;
        private readonly IDataAccessService _dataAccess;
        private readonly IUserService _userService;
        private UserModel _loggedInUser;
        private TicketModel _ticket;

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
        public UserNewTicketVM(IDataAccessService dataAccess, IUserService userService)
        {
            _dataAccess = dataAccess;
            _userService = userService;
            LoadUserDetails();

            _ticket = new TicketModel
            {
                TicketId = "1011231", // Default status for a new user is set to 'Active'.
                DeptId = 1 // Default date created is set to the current date and time.
            };

            // Initialize CreateTicketCommand with actions to execute and conditions when to be executable.
            CreateTicketCommand = new RelayCommand(CreateTicket, CanCreateTicket);
        }

        // Public property to get and set the TicketModel. Raises property changed notifications.
        public TicketModel Ticket
        {
            get => _ticket;
            set
            {
                if (_ticket != value)
                {
                    _ticket = value;
                    OnPropertyChanged(nameof(Ticket));
                    // Notify that the CreateTicketCommand may need to reevaluate its executable status.
                    CreateTicketCommand.RaiseCanExecuteChanged();
                }
            }
        }

        // Method to determine if the RegisterCommand can execute based on the current state of the UserModel properties.
        private bool CanCreateTicket(object parameter)
        {
            // Validation logic to check if message was entered.
            bool CanCreateTicket =
                !string.IsNullOrWhiteSpace(Ticket?.Message);

            return CanCreateTicket;
        }

        // Method to handle ticket creation
        private void CreateTicket(object parameter)
        {
            Debug.WriteLine("Create Ticket method called");
            try
            {
                //Perhaps put in logic here to auto-generate ticketID 
                
                // Try to register the user using the data access layer.
                var success = _dataAccess.CreateNewTicket(Ticket);
                if (success)
                {
                    MessageBox.Show("New ticket successfully!");
                    // Further actions after successful registration can be added here.
                }
                else
                {
                    MessageBox.Show("Ticket creation failed. Please try again.");
                }
            }
            catch (Exception ex)
            {
                // In case of an error, log the exception details.
                Debug.WriteLine($"Registration failed: {ex.Message}");
            }
        }

        // Command property to be bound to a create ticket button.
        public RelayCommand CreateTicketCommand { get; private set; }

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