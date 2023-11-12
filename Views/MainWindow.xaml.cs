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
    /// Interaction logic for MainWindow.xaml
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Retrieve the nav service from IoC container
            var navigationService = IoCContainer.GetService<INavigationService>();

            // Set main frame
            navigationService.SetMainFrame(MainFrame);

            // Init WelcomePage with ViewModel
            var welcomePageVM = new WelcomePageVM(navigationService);
            var welcomePage = new WelcomePage
            {
                DataContext = welcomePageVM
            };

            // Nav to WelcomePage using frame.
            MainFrame.Navigate(welcomePage);
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            // Clear previous items
            SidebarStackPanel.Children.Clear();

            if (e.Content is NewUser)
            {
                // Create button that looks like hyperlink
                Button loginButton = new Button
                {
                    Content = "Go to User Login",
                    Style = (Style)FindResource("LinkButtonStyle")
                };

                // Set Click event handler for button
                loginButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new UserLogin());
                };

                // Add button to sidebar
                SidebarStackPanel.Children.Add(loginButton);
            }
            // Start UserDash button links
            else if (e.Content is UserDash)
            {
                // Sidebar Heading for Helpdesk Tools
                TextBlock heading = new TextBlock
                {
                    Text = "Helpdesk Tools",
                    Style = (Style)FindResource("SidebarHeadingStyle")
                };
                SidebarStackPanel.Children.Add(heading);

                // Create DashBoard button link
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

                // Create New Ticket button link
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

                // End of UserDash button links
            }
            // Start UserNewTicket button links
            else if (e.Content is UserNewTicket)
            {
                // Sidebar Heading for Helpdesk Tools
                TextBlock heading = new TextBlock
                {
                    Text = "Helpdesk Tools",
                    Style = (Style)FindResource("SidebarHeadingStyle")
                };
                SidebarStackPanel.Children.Add(heading);

                // Create DashBoard button link
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

                // Create New Ticket button link
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
                // End of UserNewTicket button links
            }
            // Start UserTicketReplies button links
            else if (e.Content is UserTicketReplies)
            {
                // Sidebar Heading for Helpdesk Tools
                TextBlock heading = new TextBlock
                {
                    Text = "Helpdesk Tools",
                    Style = (Style)FindResource("SidebarHeadingStyle")
                };
                SidebarStackPanel.Children.Add(heading);

                // Create DashBoard button link
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

                // Create New Ticket button link
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
                // End of UserNewTicket button links
            }
        }

    }
}