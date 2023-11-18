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
    public class UserTicketRepliesVM : ViewModelBaseClass
    {
        private readonly IDataAccessService _dataAccess;
        private readonly IUserService _userService;
        private readonly ITicketService _ticketService;
        private readonly INavigationService _navigationService;
        private UserModel _loggedInUser;
        private TicketModel _currentTicket;
        private TicketReplyModel _ticketreply;

        public ObservableCollection<TicketReplyModel> Replies { get; set; }
        public RelayCommand UpdateTicketCommand { get; private set; }
        public RelayCommand NavigateToUserDashCommand { get; private set; }
        public RelayCommand CloseTicketCommand { get; private set; }
        public RelayCommand MarkTicketResolvedCommand { get; private set; }


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
                OnPropertyChanged(nameof(CurrentTicket));   // Notify view of the property change
                LoadTicketReplies(_currentTicket.TicketId); // Load replies when the current ticket changes
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

            // initialise the Replies property
            Replies = new ObservableCollection<TicketReplyModel>();

            NavigateToUserDashCommand = new RelayCommand(ExecuteNavigateToUserDash);

            CloseTicketCommand = new RelayCommand(CloseTicket);
            MarkTicketResolvedCommand = new RelayCommand(MarkTicketResolved);

            UpdateTicketCommand = new RelayCommand(UpdateTicket, CanUpdateTicket);
        }

        private void CloseTicket(object parameter)
        {
            CurrentTicket.TicketStatus = "Closed";
            UpdateTicket();
            NavigateToUserDash();
        }

        private void MarkTicketResolved(object parameter)
        {
            CurrentTicket.TicketStatus = "Closed";
            CurrentTicket.IncidentStatus = "Resolved";
            UpdateTicket();
            NavigateToUserDash();
        }

        private void UpdateTicket()
        {
            _dataAccess.UpdateTicketStatus(CurrentTicket);
        }

        private void NavigateToUserDash()
        {
            _navigationService.NavigateTo("UserDash");
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


            Debug.WriteLine("Update Original Ticket Model");
            try
            {
                var userorigticketReply = new TicketModel
                {
                    TicketId = CurrentTicket.TicketId,
                    TicketStatus = "User Reply",
                    IncidentStatus = "Not Resolved",
                    LastReply = DateTime.Now,
                    ReplyTime = DateTime.Now
                };

                // Use the data access layer to update the original ticket.
                var success = _dataAccess.UserOriginalUpdateTicket(userorigticketReply);
                if (success)
                {
                    //MessageBox.Show("Admin Updated the tikcet");
                    //Implement logging here.
                    _navigationService.NavigateTo("UserDash");

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
            // Retrieve CurrentTicket details by ID
            CurrentTicket = _dataAccess.GetTicketDetails(ticketId);

            // Set the properties for original ticket
            if (CurrentTicket != null)
            {
                OrigPostedBy = CurrentTicket.Name;
                OrigPostedDate = $"Posted today at {CurrentTicket.Date:HH:mm}";
                OrigMessage = CurrentTicket.Message;
            }
        }

    }
}