using System;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace HelpHive.Services
{
    public class AppNavigationService : INavigationService
    {
        private readonly Frame _frame;

        public AppNavigationService(Frame frame)
        {
            _frame = frame ?? throw new ArgumentNullException(nameof(frame));
        }

        public AppNavigationService(NavigationService navigationService)
        {
        }

        public void NavigateTo(string pageKey)
        {
            switch (pageKey)
            {
                case "NewUser":
                    _frame.Navigate(new Uri("/Views/Pages/NewUser.xaml", UriKind.Relative));
                    break;
                case "UserLogin":
                    _frame.Navigate(new Uri("/Views/Pages/UserLogin.xaml", UriKind.Relative));
                    break;
                case "UserDash":
                    _frame.Navigate(new Uri("/Views/Pages/UserDash.xaml", UriKind.Relative));
                    break;
                // Add cases for other pages as needed
                default:
                    throw new ArgumentException("Unknown page key", nameof(pageKey));
            }
        }

        public void GoBack()
        {
            if (_frame.CanGoBack)
            {
                _frame.GoBack();
            }
        }
    }
}
