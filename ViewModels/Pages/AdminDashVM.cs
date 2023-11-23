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
    public class AdminDashVM : ViewModelBaseClass
    {
        private readonly IDataAccessService _dataAccess;
        private readonly IAdminService _adminService;
        private AdminModel _loggedInAdmin;
        public ObservableCollection<TicketModel> AdminOpenTickets { get; private set; }

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

        // Constructor
        public AdminDashVM(IDataAccessService dataAccess, IAdminService adminService)
        {
            _dataAccess = dataAccess;
            _adminService = adminService;
            AdminOpenTickets = new ObservableCollection<TicketModel>(); //NB!
            FilteredTickets = new ObservableCollection<TicketModel>();

            LoadAdminDetails();
            GetOpenTicketsAsAdmin();
        }

        // filtering tickets
        private ObservableCollection<TicketModel> _filteredTickets;
        public ObservableCollection<TicketModel> FilteredTickets
        {
            get { return _filteredTickets; }
            set
            {
                _filteredTickets = value;
                OnPropertyChanged(nameof(FilteredTickets));
            }
        }

        // filter tickets based on user's search term
        public void FilterTickets(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                FilteredTickets = new ObservableCollection<TicketModel>(AdminOpenTickets);
            }
            else
            {
                FilteredTickets = new ObservableCollection<TicketModel>(
                    AdminOpenTickets.Where(ticket => ticket.MatchesSearch(searchText)));
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
        // Method to load tickets
        private void GetOpenTicketsAsAdmin()
        {
            var tickets = _dataAccess.GetOpenTicketsAsAdmin();

            AdminOpenTickets.Clear(); // Clear existing collection
            foreach (var ticket in tickets)
            {
                AdminOpenTickets.Add(ticket); // Add items to existing collection
            }

            // Initialise FilteredTickets with all tickets to display them by default
            FilterTickets(""); // Pass an empty string to show all tickets initially
        }

    }

}