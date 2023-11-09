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

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            SidebarStackPanel.Children.Clear(); // Clear previous items

            if (e.Content is NewUser)
            {
                // Create a Button that looks like a Hyperlink
                Button loginButton = new Button
                {
                    Content = "Go to User Login",
                    Style = (Style)FindResource("LinkButtonStyle") // Make sure you define this style in your resources
                };

                // Set the Click event handler for the button
                loginButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new UserLogin());
                };

                // Add the button to the sidebar
                SidebarStackPanel.Children.Add(loginButton);
            }
            // Start of UserDash Button Links
            else if (e.Content is UserDash)
            {
                // Sidebar Heading for Helpdesk Tools
                TextBlock heading = new TextBlock
                {
                    Text = "Helpdesk Tools",
                    Style = (Style)FindResource("SidebarHeadingStyle")
                };
                SidebarStackPanel.Children.Add(heading);

                // Create DashBoard Button Link
                Button DashBoardButton = new Button
                {
                    Content = "DashBoard",
                    Style = (Style)FindResource("SidebarButtonStyle")
                };
                DashBoardButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new UserDash());
                };
                SidebarStackPanel.Children.Add(DashBoardButton);

                // Create New Ticket Button Link
                Button newTicketButton = new Button
                {
                    Content = "Create New Ticket",
                    Style = (Style)FindResource("SidebarButtonStyle")
                };
                newTicketButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new UserNewTicket());
                };
                SidebarStackPanel.Children.Add(newTicketButton);

                // End of UserDash Button Links
            }
            // Start Of UserNewTicket Button Links
            else if (e.Content is UserNewTicket)
            {
                // Sidebar Heading for Helpdesk Tools
                TextBlock heading = new TextBlock
                {
                    Text = "Helpdesk Tools",
                    Style = (Style)FindResource("SidebarHeadingStyle")
                };
                SidebarStackPanel.Children.Add(heading);

                // Create DashBoard Button Link
                Button DashBoardButton = new Button
                {
                    Content = "DashBoard",
                    Style = (Style)FindResource("SidebarButtonStyle")
                };
                DashBoardButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new UserDash());
                };
                SidebarStackPanel.Children.Add(DashBoardButton);

                // Create New Ticket Button Link
                Button newTicketButton = new Button
                {
                    Content = "Create New Ticket",
                    Style = (Style)FindResource("SidebarButtonStyle")
                };
                newTicketButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new UserNewTicket());
                };
                SidebarStackPanel.Children.Add(newTicketButton);
                // End of UserNewTicket Button Links
            }
        }




    }
}