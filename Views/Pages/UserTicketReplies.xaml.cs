using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HelpHive.Models;
using HelpHive.Services;
using HelpHive.Utilities;
using HelpHive.ViewModels.Pages;

namespace HelpHive.Views.Pages
{
    /// Interaction logic for UserTicketReplies.xaml
    public partial class UserTicketReplies : Page
    {
        private string _ticketId;
        private string _staticDepartmentName;
        private string _staticTicketStatus;
        private string _staticIncidentStatus;
        private UserTicketRepliesVM _viewModel;

        // Constructor that takes tid
        public UserTicketReplies(string ticketId)
        {
            InitializeComponent();

            _ticketId = ticketId;

            IDataAccessService dataAccess = IoCContainer.GetService<IDataAccessService>();
            IUserService userService = IoCContainer.GetService<IUserService>();
            ITicketService ticketService = IoCContainer.GetService<ITicketService>();
            INavigationService navigationService = IoCContainer.GetService<INavigationService>();

            // Instantiate the ViewModel and set it as the DataContext for this page
            _viewModel = new UserTicketRepliesVM(dataAccess, userService, navigationService);
            this.DataContext = _viewModel;

            // Load the ticket details into the ViewModel
            _viewModel.LoadTicketDetails(ticketId);

            // Capture static values after loading ticket details
            _staticDepartmentName = _viewModel.CurrentTicket.DepartmentName;
            _staticTicketStatus = _viewModel.CurrentTicket.TicketStatus;
            _staticIncidentStatus = _viewModel.CurrentTicket.IncidentStatus;

            // Set the text of the TextBlock
            DepartmentTextBlock.Text = $"Department       : {_staticDepartmentName}";
            TicketStatusTextBlock.Text = $"Ticket Status      : {_staticTicketStatus}";
            IncidentStatusTextBlock.Text = $"Incident Status   : {_staticIncidentStatus}";
        }

        private void TxtUpdateMessage_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && textBox.Text == "Enter your update message here...")
            {
                textBox.Text = string.Empty;

                textBox.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void TxtUpdateMessage_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                // Set to light color to indicate placeholder text
                textBox.Foreground = new SolidColorBrush(Colors.Gray);
                textBox.Text = "Enter your update message here...";
            }
        }

    }
}
