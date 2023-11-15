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
using System.Collections.ObjectModel;

namespace HelpHive.ViewModels.Pages
{
    public class AdminTicketRepliesVM : ViewModelBaseClass
    {
        private readonly IDataAccessService _dataAccess;
        private readonly IAdminService _adminService;
        private readonly ITicketService _ticketService;
        private readonly INavigationService _navigationService;
        private AdminModel _loggedInAdmin;
        private TicketModel _currentTicket;
        private TicketReplyModel _ticketreply;


        public ObservableCollection<TicketReplyModel> Replies { get; set; }
        public RelayCommand UpdateTicketCommand { get; private set; }
        public RelayCommand NavigateToUserDashCommand { get; private set; }

        // Bindable property for the View
        public AdminModel LoggedInAdmin
        {
            get { return _loggedInAdmin; }
            set
            {
                _loggedInAdmin = value;
                OnPropertyChanged(nameof(LoggedInAdmin)); // Notify view of the property change
            }
        }

        // Bindable property for the current ticket
        public TicketModel CurrentTicket
        {
            get { return _currentTicket; }
            set
            {
                _currentTicket = value;
                OnPropertyChanged(nameof(CurrentTicket));   // Notify view of the property change
                LoadTicketReplies(_currentTicket.TicketId); // Load replies when the current ticket changes
            }
        }

        // Constructor
        public AdminTicketRepliesVM(IDataAccessService dataAccess, IAdminService adminService, INavigationService navigationService)
        {
            _dataAccess = dataAccess;
            _adminService = adminService;
            _navigationService = navigationService;
            //_ticketService = ticketService;
            //UserTicketReplies = new ObservableCollection<TicketReplies>(); //NB!
            LoadAdminDetails();
            //LoadTicketReplies();

            // initialise the Replies property
            Replies = new ObservableCollection<TicketReplyModel>();

            //Looking into the issue below
            //NavigateToAdminDashCommand = new RelayCommand(ExecuteNavigateToAdminDash);

            //Admin List from DB
            // Init the ObservableCollection
            AdminList = new ObservableCollection<AdminModel>();
            // Load admins from the database
            LoadAdminList();

            UpdateTicketCommand = new RelayCommand(UpdateTicket, CanUpdateTicket);
        }

        // method to call the GetTicketReplies method and populate the Replies property
        public void LoadTicketReplies(string ticketId)
        {
            var repliesList = _dataAccess.GetTicketReplies(ticketId);
            Replies.Clear();
            foreach (var reply in repliesList)
            {
                Replies.Add(reply);
            }
        }

        private bool CanUpdateTicket(object parameter)
        {
            // Updated validation logic
            return !string.IsNullOrWhiteSpace(UserMessage);
        }

        // Method to handle ticket update
        private void UpdateTicket(object parameter)
        {
            Debug.WriteLine("Update Ticket method called");
            try
            {
                var adminticketReply = new TicketReplyModel
                {
                    Tid = CurrentTicket.TicketId,
                    Admin = LoggedInAdmin.FirstName + " " + LoggedInAdmin.LastName,
                    Email = LoggedInAdmin.Email,
                    Date = DateTime.Now,
                    Message = this.UserMessage,
                    // Set Rating - Implement if time permits
                };

                // Use the data access layer to save the new ticket
                var success = _dataAccess.InsertAdminTicketReply(ticketReply);
                if (success)
                {
                    MessageBox.Show("New Admin reply");

                    _navigationService.NavigateTo("AdminDash");

                }
                else
                {
                    MessageBox.Show("Ticket update failed. Please check the entered information and try again.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while creating the ticket. Please try again later.");
                Debug.WriteLine($"Ticket creation failed: {ex.Message}");
            }
        }

        public string AdminFullName
        {
            get { return LoggedInAdmin.FirstName + " " + LoggedInAdmin.LastName; }
        }

        private void ExecuteNavigateToAdminDash(object parameter)
        {
            _navigationService.NavigateTo("AdminDash");
        }


        private string _adminMessage;
        private TicketReplyModel ticketReply;

        public string UserMessage
        {
            get { return _adminMessage; }
            set
            {
                if (_adminMessage != value)
                {
                    _adminMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        // Method to load admin details
        public void LoadAdminDetails()
        {
            if (_adminService.CurrentAdmin != null)
            {
                LoggedInAdmin = _dataAccess.GetAdminDetails(_adminService.CurrentAdmin.Email);
            }
        }

        // Method to load ticket details. Call this method from the view's code-behind
        public void LoadTicketDetails(string ticketId)
        {
            // Method to retrieve CurrentTicket details by ID
            CurrentTicket = _dataAccess.GetTicketDetails(ticketId);
        }







        //Populate Administrators collection from DB
        private void LoadAdminList()
        {
            var adminList = _dataAccess.GetAdmins(); // Calling GetAdmin
            foreach (var admn in adminList)
            {
                AdminList.Add(admn);
            }
        }



        // Collection of Admin prop to hold the selected department's ID
        public ObservableCollection<AdminModel> AdminList { get; set; }

        private int _selectedAdminId;
        public int SelectedAdminId
        {
            get => _selectedAdminId;
            set
            {
                if (_selectedAdminId != value)
                {
                    _selectedAdminId = value;
                    OnPropertyChanged(nameof(SelectedAdminId));
                    // Re-evaluate the CanExecute of the command
                    UpdateTicketCommand.RaiseCanExecuteChanged();
                }
            }
        }




    }
}