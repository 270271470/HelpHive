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
        private TicketModel _adminogupdate;
        private readonly ILoggingService _loggingService;

        public ObservableCollection<TicketReplyModel> Replies { get; set; }
        public RelayCommand UpdateTicketCommand { get; private set; }
        public RelayCommand NavigateToAdminDashCommand { get; private set; }
        public RelayCommand CloseTicketCommand { get; private set; }
        public RelayCommand MarkTicketResolvedCommand { get; private set; }

        // Constructor
        public AdminTicketRepliesVM(IDataAccessService dataAccess, IAdminService adminService, INavigationService navigationService, ILoggingService loggingService)
        {
            _dataAccess = dataAccess;
            _adminService = adminService;
            _navigationService = navigationService;
            _loggingService = loggingService;
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

            // Set Amin FullName
            if (_adminService.CurrentAdmin != null)
            {
                // Wait until AdminList is populated before setting the selected admin
                SelectedAdminFullName = AdminList.FirstOrDefault(a =>
                    a.FirstName == _adminService.CurrentAdmin.FirstName &&
                    a.LastName == _adminService.CurrentAdmin.LastName)?.FullName;
            }

            CloseTicketCommand = new RelayCommand(CloseTicket);
            MarkTicketResolvedCommand = new RelayCommand(MarkTicketResolved);

            UpdateTicketCommand = new RelayCommand(UpdateTicket, CanUpdateTicket);
        }



        private string _origPostedBy;
        public string OrigPostedBy
        {
            get { return _origPostedBy; }
            set
            {
                _origPostedBy = value;
                OnPropertyChanged(nameof(OrigPostedBy));
            }
        }

        private string _OrigPostedDate;
        public string OrigPostedDate
        {
            get { return _OrigPostedDate; }
            set
            {
                _OrigPostedDate = value;
                OnPropertyChanged(nameof(OrigPostedDate));
            }
        }

        private string _OrigMessage;
        public string OrigMessage
        {
            get { return _OrigMessage; }
            set
            {
                _OrigMessage = value;
                OnPropertyChanged(nameof(OrigMessage));
            }
        }


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

        private string _selectedAdminFullName;
        public string SelectedAdminFullName
        {
            get { return _selectedAdminFullName; }
            set
            {
                if (_selectedAdminFullName != value)
                {
                    _selectedAdminFullName = value;
                    OnPropertyChanged(nameof(SelectedAdminFullName)); // This notifies the UI to update
                }
            }
        }

        private void CloseTicket(object parameter)
        {
            CurrentTicket.TicketStatus = "Closed";
            UpdateTicket();
            _loggingService.Log($"ADMIN - {LoggedInAdmin.FullName} (ID {LoggedInAdmin.AdminId}) marked Ticket ID {CurrentTicket.TicketId} as CLOSED", LogLevel.Info);
            NavigateToAdminDash();
        }

        private void MarkTicketResolved(object parameter)
        {
            CurrentTicket.TicketStatus = "Closed";
            CurrentTicket.IncidentStatus = "Resolved";
            UpdateTicket();
            _loggingService.Log($"ADMIN - {LoggedInAdmin.FullName} (ID {LoggedInAdmin.AdminId}) marked Ticket ID {CurrentTicket.TicketId} as RESOLVED", LogLevel.Info);
            NavigateToAdminDash();
        }

        private void UpdateTicket()
        {
            _dataAccess.UpdateTicketStatus(CurrentTicket);
        }

        private void NavigateToAdminDash()
        {
            _navigationService.NavigateTo("AdminDash");
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
            //return !string.IsNullOrWhiteSpace(UserMessage);
            return !string.IsNullOrEmpty(CurrentTicket.TicketStatus);
            //return true;
        }

        // Method to handle ticket update
        private void UpdateTicket(object parameter)
        {
            Debug.WriteLine("Update Ticket Reply Model");
            try
            {
                var adminticketReply = new TicketReplyModel
                {
                    Tid = CurrentTicket.TicketId,
                    Date = DateTime.Now,
                    Message = this.UserMessage,
                    Admin = LoggedInAdmin.FirstName + " " + LoggedInAdmin.LastName,
                };

                // Use the data access layer to save the new ticket
                var success = _dataAccess.InsertAdminTicketReply(adminticketReply);
                if (success)
                {
                    //MessageBox.Show("New Admin reply");
                    //Implement logging here.
                    _loggingService.Log($"ADMIN - {LoggedInAdmin.FullName} (ID {LoggedInAdmin.AdminId}) posted a reply to Ticket ID {CurrentTicket.TicketId}", LogLevel.Info);
                    _navigationService.NavigateTo("AdminDash");

                }
                else
                {
                    _loggingService.Log($"ADMIN - Ticket ID {CurrentTicket.TicketId} update failed!", LogLevel.Error);
                    MessageBox.Show("Ticket update failed. Please check the entered information and try again.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while creating the ticket. Please try again later.");
                Debug.WriteLine($"Ticket creation failed: {ex.Message}");
            }




            Debug.WriteLine("Update Original Ticket Model");
            try
            {
                var adminorigticketReply = new TicketModel
                {
                    TicketId = CurrentTicket.TicketId,
                    DeptId = CurrentTicket.DeptId,
                    TicketStatus = CurrentTicket.TicketStatus,
                    IncidentStatus = CurrentTicket.IncidentStatus,
                    Urgency = CurrentTicket.Urgency,
                    Admin = LoggedInAdmin.FirstName + " " + LoggedInAdmin.LastName,
                    LastReply = DateTime.Now,
                    ReplyTime = DateTime.Now
                };

                // Use the data access layer to update the original ticket.
                var success = _dataAccess.AdminOriginalUpdateTicket(adminorigticketReply);
                if (success)
                {
                    //MessageBox.Show("Admin Updated the tikcet");
                    //Implement logging here.
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
        //private TicketReplyModel ticketReply;
        //private TicketModel adminOGUpdate;

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
            // Retrieve CurrentTicket details by ID
            CurrentTicket = _dataAccess.GetTicketDetails(ticketId);

            // Set the properties for original ticket
            if (CurrentTicket != null)
            {
                CurrentTicket.TicketStatus = "Answered";
                OrigPostedBy = CurrentTicket.Name;
                //OrigPostedDate = $"Posted today at {CurrentTicket.Date:HH:mm}";
                OrigPostedDate = $"Posted on {CurrentTicket.LastReply.ToString("dddd, dd MMMM yyyy HH:mm")}";
                OrigMessage = CurrentTicket.Message;
            }
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