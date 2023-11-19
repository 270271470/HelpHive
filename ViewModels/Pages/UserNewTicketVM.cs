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
using System.Collections.ObjectModel;
using HelpHive.Utilities;
using HelpHive.Views.Pages;

namespace HelpHive.ViewModels.Pages
{
    public class UserNewTicketVM : ViewModelBaseClass
    {
        private readonly INavigationService _navigationService;
        private readonly IDataAccessService _dataAccess;
        private readonly IUserService _userService;
        private readonly ILoggingService _loggingService;
        private UserModel _loggedInUser;
        private TicketModel _ticket;

        public RelayCommand NavigateToUserDashCommand { get; private set; }



        // Constructor
        public UserNewTicketVM(IDataAccessService dataAccess, IUserService userService, ILoggingService loggingService, INavigationService navigationService)
        {
            _dataAccess = dataAccess;
            _userService = userService;
            _loggingService = loggingService;
            _navigationService = navigationService;

            // Init command and pass the method to execute
            NavigateToUserDashCommand = new RelayCommand(ExecuteNavigateToUserDash);

            LoadUserDetails();

            // Init the TicketModel
            // TicketId will be set in CreateTicket method and DeptId will be set based on user selection
            _ticket = new TicketModel {

                UserId = LoggedInUser.UserId,
                Name = LoggedInUser.FirstName + " " + LoggedInUser.LastName,
                Email = LoggedInUser.Email
            };

            //Departments from DB
            // Init the ObservableCollection
            Departments = new ObservableCollection<TicketDeptsModel>();
            // Load departments from the database
            LoadDepartments();

            // Init CreateTicketCommand with actions to execute and conditions when to be executable.
            CreateTicketCommand = new RelayCommand(CreateTicket, CanCreateTicket);
        }



        // LoggedInUser - Bindable property for the View
        public UserModel LoggedInUser
        {
            get { return _loggedInUser; }
            set
            {
                _loggedInUser = value;
                OnPropertyChanged(nameof(LoggedInUser)); // Notify view of property change
            }
        }



        // Subject of ticket
        public string Subject
        {
            get => _ticket.Title;
            set
            {
                if (_ticket.Title != value)
                {
                    _ticket.Title = value;
                    OnPropertyChanged(nameof(Subject));
                    // Re-evaluate the CanExecute of the command
                    CreateTicketCommand.RaiseCanExecuteChanged();
                }
            }
        }

        // Message of ticket
        public string Message
        {
            get => _ticket.Message;
            set
            {
                if (_ticket.Message != value)
                {
                    _ticket.Message = value;
                    OnPropertyChanged(nameof(Message));
                    // Re-evaluate the CanExecute of the command
                    CreateTicketCommand.RaiseCanExecuteChanged();
                }
            }
        }



        // Public property to get and set the TicketModel. Raises property changed notifications
        public TicketModel Ticket
        {
            get => _ticket;
            set
            {
                if (_ticket != value)
                {
                    _ticket = value;
                    OnPropertyChanged(nameof(Ticket));
                    // Notify that CreateTicketCommand may need to reevaluate its executable status
                    CreateTicketCommand.RaiseCanExecuteChanged();
                }
            }
        }



        // Method to determine if the RegisterCommand can execute based on the current state of the UserModel props
        private bool CanCreateTicket(object parameter)
        {
            // Updated validation logic to use SelectedDepartmentId - 10/11/23
            return !string.IsNullOrWhiteSpace(Subject);
        }

        private void ExecuteNavigateToUserDash(object parameter)
        {
            _navigationService.NavigateTo("UserDash");
        }


        // Method to handle ticket creation
        private void CreateTicket(object parameter)
        {
            Debug.WriteLine("Create Ticket method called");
            try
            {
                //Get current date time
                Ticket.Date = DateTime.Now;
                Ticket.LastReply = DateTime.Now;
                Ticket.ReplyTime = DateTime.Now;

                // Set the DeptId from the selected department
                Ticket.DeptId = SelectedDepartmentId;

                //Generate the TicketId
                Ticket.TicketId = GenerateTicketId(); // Set a 6-digit ID

                //Set the Urgency/Priority
                Ticket.Urgency = SelectedPriority;

                // Use the data access layer to save the new ticket
                var success = _dataAccess.CreateNewTicket(Ticket);
                if (success)
                {
                    //MessageBox.Show("New ticket created successfully!");
                    // Log the message with the TicketId
                    _loggingService.Log($"New ticket with ID {Ticket.TicketId} created successfully by UserId: {LoggedInUser.UserId}", LogLevel.Info);
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
                _loggingService.Log($"Ticket creation failed: {ex.Message}", LogLevel.Error);
                Debug.WriteLine($"Ticket creation failed: {ex.Message}");
            }
        }



        private Random _random = new Random();
        private string GenerateTicketId()
        {
            int id = _random.Next(100000, 999999); // Generate random num between 100000 and 999999
            return id.ToString();
        }

        // Command property to be bound to a create ticket button
        public RelayCommand CreateTicketCommand { get; private set; }

        // Method to load user details
        public void LoadUserDetails()
        {
            if (_userService.CurrentUser != null)
            {
                LoggedInUser = _dataAccess.GetUserDetails(_userService.CurrentUser.Email);
            }
        }



        //Populate the Departments collection from DB
        private void LoadDepartments()
        {
            var departmentList = _dataAccess.GetDepartments(); // Calling GetDepartments
            foreach (var dept in departmentList)
            {
                Departments.Add(dept);
            }
        }



        // Collection of Dept prop to hold the selected department's ID
        public ObservableCollection<TicketDeptsModel> Departments { get; set; }

        private int _selectedDepartmentId;
        public int SelectedDepartmentId
        {
            get => _selectedDepartmentId;
            set
            {
                if (_selectedDepartmentId != value)
                {
                    _selectedDepartmentId = value;
                    OnPropertyChanged(nameof(SelectedDepartmentId));
                    // Re-evaluate the CanExecute of the command
                    CreateTicketCommand.RaiseCanExecuteChanged();
                }
            }
        }



        private string _selectedPriority;
        public string SelectedPriority
        {
            get => _selectedPriority;
            set
            {
                if (_selectedPriority != value)
                {
                    _selectedPriority = value;
                    OnPropertyChanged(nameof(SelectedPriority));
                    // Re-evaluate the CanExecute of the command
                    CreateTicketCommand.RaiseCanExecuteChanged();
                }
            }
        }

    }

}