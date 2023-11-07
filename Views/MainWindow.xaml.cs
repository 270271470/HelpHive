using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HelpHive.Views.Pages;
using HelpHive.Utilities;
using HelpHive.Services;
using HelpHive.ViewModels.Pages;


// Code-behind - MainWindow

namespace HelpHive.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Retrieve the navigation service from the IoC container
            var navigationService = IoCContainer.GetService<INavigationService>();

            // Set the main frame to navigation service
            navigationService.SetMainFrame(MainFrame);

            // Initialize the WelcomePage with the ViewModel
            var welcomePageVM = new WelcomePageVM(navigationService);
            var welcomePage = new WelcomePage
            {
                DataContext = welcomePageVM
            };

            // Navigate to WelcomePage using the Frame.
            MainFrame.Navigate(welcomePage);
        }




        // Add link methods here:
        private void OnNewTicketClicked(object sender, RoutedEventArgs e)
        {
            // Handle the button click here.
            // For example, navigate to a new page or open a dialog.
        }
        private void OnActiveTicketsClicked(object sender, RoutedEventArgs e)
        {
            // Handle the button click here.
            // For example, navigate to a new page or open a dialog.
        }
        private void OnTicketHistoryClicked(object sender, RoutedEventArgs e)
        {
            // Handle the button click here.
            // For example, navigate to a new page or open a dialog.
        }

    }
}