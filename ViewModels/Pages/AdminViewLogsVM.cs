using System.Collections.ObjectModel;
using HelpHive.Models;
using HelpHive.Services;

namespace HelpHive.ViewModels.Pages
{

    // view model for admin viewing logs
    public class AdminViewLogsVM : ViewModelBaseClass
    {
        private readonly IDataAccessService _dataAccess;
        private readonly IAdminService _adminService;
        private AdminModel _loggedInAdmin;

        public ObservableCollection<LogEntry> LogEntries { get; set; }

        // constructor
        public AdminViewLogsVM(IDataAccessService dataAccess, IAdminService adminService)
        {
            _dataAccess = dataAccess;
            _adminService = adminService;
            LogEntries = new ObservableCollection<LogEntry>();

            LoadLogEntries();
        }

        // query database to get logs
        public void LoadLogEntries()
        {
            var logs = _dataAccess.GetLogEntries();
            LogEntries.Clear();
            foreach (var log in logs)
            {
                LogEntries.Add(log);
            }
            OnPropertyChanged(nameof(LogEntries)); // Notify the view that LogEntries has been updated
        }
    }
}