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

        public WelcomePageVM(INavigationService navigationService)
        {
            _navigationService = navigationService;

            // Initialize the command and pass the method to execute
            NavigateToNewUserCommand = new RelayCommand(ExecuteNavigateToNewUser);
            NavigateToUserLoginCommand = new RelayCommand(ExecuteNavigateToUserLogin);
        }

        private void ExecuteNavigateToNewUser(object parameter)
        {
            _navigationService.NavigateTo("NewUser");
        }
        private void ExecuteNavigateToUserLogin(object parameter)
        {
            _navigationService.NavigateTo("UserLogin");
        }
    }
}