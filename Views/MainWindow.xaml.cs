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
        private readonly IAdminService _adminService;
        private readonly IUserService _userService;

        public MainWindow()
        {
            InitializeComponent();

            // Retrieve services from IoC container
            var navigationService = IoCContainer.GetService<INavigationService>();
            _adminService = IoCContainer.GetService<IAdminService>();
            _userService = IoCContainer.GetService<IUserService>();

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

        // Admin Logout button click event handler
        private void AdminLogoutButton_Click(object sender, RoutedEventArgs e)
        {
            _adminService.Logout(); // Log out the current admin
            MainFrame.Navigate(new AdminLogin());
            UpdateUIForLogout();
        }

        // USer Logout button click event handler
        private void UserLogoutButton_Click(object sender, RoutedEventArgs e)
        {
            _userService.Logout(); // Log out the current user
            MainFrame.Navigate(new UserLogin());
            UpdateUIForLogout();
        }

        // Method to update UI after logout
        private void UpdateUIForLogout()
        {
            SidebarStackPanel.Children.Clear(); // Clear the sidebar
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            // Clear previous items
            SidebarStackPanel.Children.Clear();

            if (e.Content is NewUser)
            {
                // Sidebar Heading for Admin Tools
                TextBlock heading = new TextBlock
                {
                    Text = "",
                    Style = (Style)FindResource("SidebarHeadingStyle")
                };
                SidebarStackPanel.Children.Add(heading);

                // User Login
                Button UserLoginButton = new Button
                {
                    Content = "User Login",
                    Style = (Style)FindResource("SidebarButtonStyle")
                };
                UserLoginButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new UserLogin());
                };
                SidebarStackPanel.Children.Add(UserLoginButton);
                // End of UserNewTicket button links

                // User Login
                Button AdminLoginButton = new Button
                {
                    Content = "Administrator Login",
                    Style = (Style)FindResource("SidebarButtonStyle")
                };
                AdminLoginButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new AdminLogin());
                };
                SidebarStackPanel.Children.Add(AdminLoginButton);
                // End of UserNewTicket button links

                // Create New User
                Button CreateAdminButton = new Button
                {
                    Content = "Create New Admin",
                    Style = (Style)FindResource("SidebarButtonStyle")
                };
                CreateAdminButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new NewAdmin());
                };
                SidebarStackPanel.Children.Add(CreateAdminButton);
                // End of Create New User button links
            }
            // Start UserDash button links
            else if (e.Content is UserDash)
            {
                // Sidebar Heading for User Tools
                TextBlock heading = new TextBlock
                {
                    Text = "User Tools",
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

                // Create TicketHistory button link
                Button TicketHistoryButton = new Button
                {
                    Content = "Ticket History",
                    Style = (Style)FindResource("SidebarButtonStyle")
                };
                TicketHistoryButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new UserTicketHistory());
                };
                SidebarStackPanel.Children.Add(TicketHistoryButton);

                // Create UserLogout button link
                Button logoutButton = new Button
                {
                    Content = "Logout",
                    Style = (Style)FindResource("SidebarButtonStyle"),
                    Margin = new Thickness(30, 50, 0, 0)
                };

                logoutButton.Click += (s, args) =>
                {
                    _userService.Logout();
                    UpdateUIForLogout();
                    MainFrame.Navigate(new UserLogin());
                };
                SidebarStackPanel.Children.Add(logoutButton);
                // End of UserDash button links
            }
            // Start UserNewTicket button links
            else if (e.Content is UserNewTicket)
            {
                // Sidebar Heading for User Tools
                TextBlock heading = new TextBlock
                {
                    Text = "User Tools",
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

                // Create TicketHistory button link
                Button TicketHistoryButton = new Button
                {
                    Content = "Ticket History",
                    Style = (Style)FindResource("SidebarButtonStyle")
                };
                TicketHistoryButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new UserTicketHistory());
                };
                SidebarStackPanel.Children.Add(TicketHistoryButton);

                // Create UserLogout button link
                Button logoutButton = new Button
                {
                    Content = "Logout",
                    Style = (Style)FindResource("SidebarButtonStyle"),
                    Margin = new Thickness(30, 50, 0, 0)
                };

                logoutButton.Click += (s, args) =>
                {
                    _userService.Logout();
                    UpdateUIForLogout();
                    MainFrame.Navigate(new UserLogin());
                };
                SidebarStackPanel.Children.Add(logoutButton);

                // End of UserNewTicket button links
            }
            // Start UserTicketReplies button links
            else if (e.Content is UserTicketReplies)
            {
                // Sidebar Heading for User Tools
                TextBlock heading = new TextBlock
                {
                    Text = "User Tools",
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

                // Create TicketHistory button link
                Button TicketHistoryButton = new Button
                {
                    Content = "Ticket History",
                    Style = (Style)FindResource("SidebarButtonStyle")
                };
                TicketHistoryButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new UserTicketHistory());
                };
                SidebarStackPanel.Children.Add(TicketHistoryButton);

                // Create UserLogout button link
                Button logoutButton = new Button
                {
                    Content = "Logout",
                    Style = (Style)FindResource("SidebarButtonStyle"),
                    Margin = new Thickness(30, 50, 0, 0)
                };

                logoutButton.Click += (s, args) =>
                {
                    _userService.Logout();
                    UpdateUIForLogout();
                    MainFrame.Navigate(new UserLogin());
                };
                SidebarStackPanel.Children.Add(logoutButton);

                // End of UserNewTicket button links
            }

            // Start UserTicketHistory button links
            else if (e.Content is UserTicketHistory)
            {
                // Sidebar Heading for User Tools
                TextBlock heading = new TextBlock
                {
                    Text = "User Tools",
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

                // Create TicketHistory button link
                Button TicketHistoryButton = new Button
                {
                    Content = "Ticket History",
                    Style = (Style)FindResource("SidebarButtonStyle")
                };
                TicketHistoryButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new UserTicketHistory());
                };
                SidebarStackPanel.Children.Add(TicketHistoryButton);

                // Create UserLogout button link
                Button logoutButton = new Button
                {
                    Content = "Logout",
                    Style = (Style)FindResource("SidebarButtonStyle"),
                    Margin = new Thickness(30, 50, 0, 0)
                };

                logoutButton.Click += (s, args) =>
                {
                    _userService.Logout();
                    UpdateUIForLogout();
                    MainFrame.Navigate(new UserLogin());
                };
                SidebarStackPanel.Children.Add(logoutButton);

                // End of UserTicketHistory button links
            }

            // Start Admin Dashboard Sidebar
            else if (e.Content is AdminDash)
            {
                // Sidebar Heading for Admin Tools
                TextBlock heading = new TextBlock
                {
                    Text = "Admin Tools",
                    Style = (Style)FindResource("SidebarHeadingStyle")
                };
                SidebarStackPanel.Children.Add(heading);

                // Create DashBoard button link
                Button DashBoardButton = new Button
                {
                    Content = "Active Tickets",
                    Style = (Style)FindResource("SidebarButtonStyle")
                };
                DashBoardButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new AdminDash());
                };
                SidebarStackPanel.Children.Add(DashBoardButton);

                // Create Historical button link
                Button TicketHistoryButton = new Button
                {
                    Content = "Ticket History",
                    Style = (Style)FindResource("SidebarButtonStyle")
                };
                TicketHistoryButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new AdminTicketHistory());
                };
                SidebarStackPanel.Children.Add(TicketHistoryButton);

                // Create Logs button link
                Button LogsButton = new Button
                {
                    Content = "Logs",
                    Style = (Style)FindResource("SidebarButtonStyle")
                };
                LogsButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new AdminViewLogs());
                };
                SidebarStackPanel.Children.Add(LogsButton);

                // Create AdminLogout button link
                Button logoutButton = new Button
                {
                    Content = "Logout",
                    Style = (Style)FindResource("SidebarButtonStyle"),
                    Margin = new Thickness(30, 50, 0, 0)
                };

                logoutButton.Click += (s, args) =>
                {
                    _adminService.Logout();
                    UpdateUIForLogout();
                    MainFrame.Navigate(new AdminLogin());
                };
                SidebarStackPanel.Children.Add(logoutButton);
                // End Admin Dashboard Sidebar
            }

            // Start AdminTicketReplies button links
            else if (e.Content is AdminTicketReplies)
            {
                // Sidebar Heading for Admin Tools
                TextBlock heading = new TextBlock
                {
                    Text = "Admin Tools",
                    Style = (Style)FindResource("SidebarHeadingStyle")
                };
                SidebarStackPanel.Children.Add(heading);

                // Create DashBoard button link
                Button DashBoardButton = new Button
                {
                    Content = "Active Tickets",
                    Style = (Style)FindResource("SidebarButtonStyle")
                };
                DashBoardButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new AdminDash());
                };
                SidebarStackPanel.Children.Add(DashBoardButton);

                // Create Historical button link
                Button TicketHistoryButton = new Button
                {
                    Content = "Ticket History",
                    Style = (Style)FindResource("SidebarButtonStyle")
                };
                TicketHistoryButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new AdminTicketHistory());
                };
                SidebarStackPanel.Children.Add(TicketHistoryButton);

                // Create Logs button link
                Button LogsButton = new Button
                {
                    Content = "Logs",
                    Style = (Style)FindResource("SidebarButtonStyle")
                };
                LogsButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new AdminViewLogs());
                };
                SidebarStackPanel.Children.Add(LogsButton);

                // Create AdminLogout button link
                Button logoutButton = new Button
                {
                    Content = "Logout",
                    Style = (Style)FindResource("SidebarButtonStyle"),
                    Margin = new Thickness(30, 50, 0, 0)
                };

                logoutButton.Click += (s, args) =>
                {
                    _adminService.Logout();
                    UpdateUIForLogout();
                    MainFrame.Navigate(new AdminLogin());
                };
                SidebarStackPanel.Children.Add(logoutButton);

                // End of UserNewTicket button links
            }


            // Start AdminTicketHistory button links
            else if (e.Content is AdminTicketHistory)
            {
                // Sidebar Heading for Admin Tools
                TextBlock heading = new TextBlock
                {
                    Text = "Admin Tools",
                    Style = (Style)FindResource("SidebarHeadingStyle")
                };
                SidebarStackPanel.Children.Add(heading);

                // Create DashBoard button link
                Button DashBoardButton = new Button
                {
                    Content = "Active Tickets",
                    Style = (Style)FindResource("SidebarButtonStyle")
                };
                DashBoardButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new AdminDash());
                };
                SidebarStackPanel.Children.Add(DashBoardButton);

                // Create Historical button link
                Button TicketHistoryButton = new Button
                {
                    Content = "Ticket History",
                    Style = (Style)FindResource("SidebarButtonStyle")
                };
                TicketHistoryButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new AdminTicketHistory());
                };
                SidebarStackPanel.Children.Add(TicketHistoryButton);

                // Create Logs button link
                Button LogsButton = new Button
                {
                    Content = "Logs",
                    Style = (Style)FindResource("SidebarButtonStyle")
                };
                LogsButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new AdminViewLogs());
                };
                SidebarStackPanel.Children.Add(LogsButton);

                // Create AdminLogout button link
                Button logoutButton = new Button
                {
                    Content = "Logout",
                    Style = (Style)FindResource("SidebarButtonStyle"),
                    Margin = new Thickness(30, 50, 0, 0)
                };

                logoutButton.Click += (s, args) =>
                {
                    _adminService.Logout();
                    UpdateUIForLogout();
                    MainFrame.Navigate(new AdminLogin());
                };
                SidebarStackPanel.Children.Add(logoutButton);

                // End of UserNewTicket button links
            }

            // Start AdminViewLogs button links
            else if (e.Content is AdminViewLogs)
            {
                // Sidebar Heading for Admin Tools
                TextBlock heading = new TextBlock
                {
                    Text = "Admin Tools",
                    Style = (Style)FindResource("SidebarHeadingStyle")
                };
                SidebarStackPanel.Children.Add(heading);

                // Create DashBoard button link
                Button DashBoardButton = new Button
                {
                    Content = "Active Tickets",
                    Style = (Style)FindResource("SidebarButtonStyle")
                };
                DashBoardButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new AdminDash());
                };
                SidebarStackPanel.Children.Add(DashBoardButton);

                // Create Historical button link
                Button TicketHistoryButton = new Button
                {
                    Content = "Ticket History",
                    Style = (Style)FindResource("SidebarButtonStyle")
                };
                TicketHistoryButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new AdminTicketHistory());
                };
                SidebarStackPanel.Children.Add(TicketHistoryButton);

                // Create Logs button link
                Button LogsButton = new Button
                {
                    Content = "Logs",
                    Style = (Style)FindResource("SidebarButtonStyle")
                };
                LogsButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new AdminViewLogs());
                };
                SidebarStackPanel.Children.Add(LogsButton);

                // Create AdminLogout button link
                Button logoutButton = new Button
                {
                    Content = "Logout",
                    Style = (Style)FindResource("SidebarButtonStyle"),
                    Margin = new Thickness(30, 50, 0, 0)
                };

                logoutButton.Click += (s, args) =>
                {
                    _adminService.Logout();
                    UpdateUIForLogout();
                    MainFrame.Navigate(new AdminLogin());
                };
                SidebarStackPanel.Children.Add(logoutButton);

                // End of AdminViewLogs button links
            }




            // Start WelcomeScreen button links
            else if (e.Content is AdminLogin)
            {
                // Sidebar Heading for Admin Tools
                TextBlock heading = new TextBlock
                {
                    Text = "",
                    Style = (Style)FindResource("SidebarHeadingStyle")
                };
                SidebarStackPanel.Children.Add(heading);

                // User Login
                Button UserLoginButton = new Button
                {
                    Content = "User Login",
                    Style = (Style)FindResource("SidebarButtonStyle")
                };
                UserLoginButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new UserLogin());
                };
                SidebarStackPanel.Children.Add(UserLoginButton);
                // End of UserNewTicket button links

                // Create New User
                Button CreateUserButton = new Button
                {
                    Content = "Create New User",
                    Style = (Style)FindResource("SidebarButtonStyle")
                };
                CreateUserButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new NewUser());
                };
                SidebarStackPanel.Children.Add(CreateUserButton);
                // End of Create New User button links

                // Create New User
                Button CreateAdminButton = new Button
                {
                    Content = "Create New Admin",
                    Style = (Style)FindResource("SidebarButtonStyle")
                };
                CreateAdminButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new NewAdmin());
                };
                SidebarStackPanel.Children.Add(CreateAdminButton);
                // End of Create New User button links

            }


            else if (e.Content is UserLogin)
            {
                // Sidebar Heading for Admin Tools
                TextBlock heading = new TextBlock
                {
                    Text = "",
                    Style = (Style)FindResource("SidebarHeadingStyle")
                };
                SidebarStackPanel.Children.Add(heading);

                // Create New User
                Button CreateUserButton = new Button
                {
                    Content = "Create New User",
                    Style = (Style)FindResource("SidebarButtonStyle")
                };
                CreateUserButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new NewUser());
                };
                SidebarStackPanel.Children.Add(CreateUserButton);
                // End of Create New User button links

                // Admin Login
                Button AdminLoginButton = new Button
                {
                    Content = "Administrator Login",
                    Style = (Style)FindResource("SidebarButtonStyle")
                };
                AdminLoginButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new AdminLogin());
                };
                SidebarStackPanel.Children.Add(AdminLoginButton);
                // End of UserNewTicket button links

                // Create New Admin
                Button CreateAdminButton = new Button
                {
                    Content = "Create New Admin",
                    Style = (Style)FindResource("SidebarButtonStyle")
                };
                CreateAdminButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new NewAdmin());
                };
                SidebarStackPanel.Children.Add(CreateAdminButton);
                // End of Create New User button links

            }






            else if (e.Content is NewAdmin)
            {
                // Sidebar Heading for Admin Tools
                TextBlock heading = new TextBlock
                {
                    Text = "",
                    Style = (Style)FindResource("SidebarHeadingStyle")
                };
                SidebarStackPanel.Children.Add(heading);

                // User Login
                Button UserLoginButton = new Button
                {
                    Content = "User Login",
                    Style = (Style)FindResource("SidebarButtonStyle")
                };
                UserLoginButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new UserLogin());
                };
                SidebarStackPanel.Children.Add(UserLoginButton);
                // End of UserNewTicket button links

                // Create New User
                Button CreateUserButton = new Button
                {
                    Content = "Create New User",
                    Style = (Style)FindResource("SidebarButtonStyle")
                };
                CreateUserButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new NewUser());
                };
                SidebarStackPanel.Children.Add(CreateUserButton);
                // End of Create New User button links

                // User Login
                Button AdminLoginButton = new Button
                {
                    Content = "Administrator Login",
                    Style = (Style)FindResource("SidebarButtonStyle")
                };
                AdminLoginButton.Click += (s, args) =>
                {
                    MainFrame.Navigate(new AdminLogin());
                };
                SidebarStackPanel.Children.Add(AdminLoginButton);
                // End of UserNewTicket button links


            }





        }

    }
}