//using System;
using System.Windows.Input;     // Needed for ICommand
using HelpHive.Commands;        // Custom Commands like RelayCommand
//using System.Windows.Controls;  // Required for the Frame class

using HelpHive.Services;

namespace HelpHive.ViewModels.Pages
{
    public class WelcomePageVM
    {
        private readonly INavigationService _navigationService;

        public RelayCommand NavigateToNewUserCommand { get; private set; }
        public RelayCommand NavigateToUserLoginCommand { get; private set; }
        public RelayCommand NavigateToNewAdminCommand { get; private set; }
        public RelayCommand NavigateToAdminLoginCommand { get; private set; }

        public WelcomePageVM(INavigationService navigationService)
        {
            _navigationService = navigationService;

            // Init command and pass the method to execute
            NavigateToNewUserCommand = new RelayCommand(ExecuteNavigateToNewUser);
            NavigateToNewAdminCommand = new RelayCommand(ExecuteNavigateToNewAdmin);
            NavigateToUserLoginCommand = new RelayCommand(ExecuteNavigateToUserLogin);
            NavigateToAdminLoginCommand = new RelayCommand(ExecuteNavigateToAdminLogin);
        }

        private void ExecuteNavigateToNewUser(object parameter)
        {
            _navigationService.NavigateTo("NewUser");
        }
        private void ExecuteNavigateToNewAdmin(object parameter)
        {
            _navigationService.NavigateTo("NewAdmin");
        }
        private void ExecuteNavigateToUserLogin(object parameter)
        {
            _navigationService.NavigateTo("UserLogin");
        }
        private void ExecuteNavigateToAdminLogin(object parameter)
        {
            _navigationService.NavigateTo("AdminLogin");
        }
    }
}