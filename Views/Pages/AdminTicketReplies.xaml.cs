﻿using System;
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
using HelpHive.Services;
using HelpHive.Utilities;
using HelpHive.ViewModels.Pages;

namespace HelpHive.Views.Pages
{
    /// <summary>
    /// Interaction logic for AdminTicketReplies.xaml
    /// </summary>
    public partial class AdminTicketReplies : Page
    {
        private string _ticketId;
        private AdminTicketRepliesVM _viewModel;

        // Constructor that takes tid
        public AdminTicketReplies(string ticketId)
        {
            InitializeComponent();

            _ticketId = ticketId;

            IDataAccessService dataAccess = IoCContainer.GetService<IDataAccessService>();
            IAdminService adminService = IoCContainer.GetService<IAdminService>();
            ITicketService ticketService = IoCContainer.GetService<ITicketService>();
            INavigationService navigationService = IoCContainer.GetService<INavigationService>();

            // Instantiate the ViewModel and set it as the DataContext for this page
            _viewModel = new AdminTicketRepliesVM(dataAccess, adminService, navigationService);
            this.DataContext = _viewModel;

            // Load the ticket details into the ViewModel
            _viewModel.LoadTicketDetails(ticketId);
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