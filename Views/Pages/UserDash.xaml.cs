﻿using HelpHive.Services;
using HelpHive.Utilities;
using HelpHive.ViewModels.Pages;
using HelpHive.Models;
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

namespace HelpHive.Views.Pages
{
    /// Interaction logic for UserDash.xaml
    public partial class UserDash : Page
    {
        public UserDash()
        {
            InitializeComponent();

            IDataAccessService dataAccess = IoCContainer.GetService<IDataAccessService>();
            IUserService userService = IoCContainer.GetService<IUserService>();

            DataContext = new UserDashVM(dataAccess, userService);
        }

        // search box filtering
        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox searchBox = sender as TextBox;
            if (searchBox.Text == "Search...")
            {
                searchBox.Text = "";
                searchBox.Foreground = Brushes.Black;
            }
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox searchBox = sender as TextBox;
            if (string.IsNullOrWhiteSpace(searchBox.Text))
            {
                searchBox.Text = "Search...";
                searchBox.Foreground = Brushes.Gray;
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchBox = sender as TextBox; // Cast the sender to a TextBox
            if (searchBox != null) // Check if the cast was successful
            {
                var viewModel = DataContext as UserDashVM; // Safely cast the DataContext to UserDashVM
                if (viewModel != null) // Check if the DataContext is of the expected type
                {
                    viewModel.FilterTickets(searchBox.Text); // Call the FilterTickets method with the current text
                }
                else
                {
                    // where DataContext is not set or not of type UserDashVM
                }
            }
        }

        // If user clicks on a subject, this will redirect to the ticket replies page
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var grid = sender as DataGrid;
            var selectedTicket = grid.SelectedItem as TicketModel;
            if (selectedTicket != null)
            {
                NavigationService.Navigate(new UserTicketReplies(selectedTicket.TicketId));
            }
        }

    }
}