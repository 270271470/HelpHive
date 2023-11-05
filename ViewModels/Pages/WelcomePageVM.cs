//using System;
using System.Windows.Input;     // Needed for ICommand
using HelpHive.Commands;        // Custom Commands like RelayCommand
//using System.Windows.Controls;  // Required for the Frame class

/*
namespace HelpHive.ViewModels.Pages
{
    public class WelcomePageVM
    {
        public ICommand NavigateToNewUserCommand { get; private set; }

        private readonly Frame _navigationFrame;

        // Constructor requires the main frame to perform navigation.
        public WelcomePageVM(Frame navigationFrame)
        {
            _navigationFrame = navigationFrame;
            NavigateToNewUserCommand = new RelayCommand(ExecuteNavigateToNewUser);
        }

        // Navigation logic to NewUser page.
        private void ExecuteNavigateToNewUser(object parameter)
        {
            _navigationFrame.Navigate(new Uri("Views/Pages/NewUser.xaml", UriKind.Relative));
        }
    }
} */


using HelpHive.Services;

namespace HelpHive.ViewModels.Pages
{
    public class WelcomePageVM
    {
        private readonly INavigationService _navigationService;

        public ICommand NavigateToNewUserCommand { get; private set; } // Changed from RelayCommand to ICommand for more general use

        public WelcomePageVM(INavigationService navigationService)
        {
            _navigationService = navigationService;
            // Initialize the command and pass the method to execute
            NavigateToNewUserCommand = new RelayCommand(ExecuteNavigateToNewUser);
        }

        private void ExecuteNavigateToNewUser(object parameter)
        {
            _navigationService.NavigateTo("NewUser");
        }
    }
}