using System.Windows.Controls;      // For Frame
using System.Windows.Navigation;    // For NavigationService

// This class implements the interface (INavigationService) and will handle the actual navigation logic.

namespace HelpHive.Services
{
    public class NavigationService : INavigationService
    {
        private readonly Frame _frame;

        public NavigationService(Frame frame)
        {
            _frame = frame;
        }

        public void NavigateTo(string pageKey)
        {
            _frame.Navigate(new System.Uri($"Views/Pages/{pageKey}.xaml", System.UriKind.Relative));
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