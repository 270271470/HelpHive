﻿using HelpHive.Models;
using HelpHive.Services;
using HelpHive.Utilities;
using HelpHive.ViewModels.Pages;
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
    /// Interaction logic for UserTicketHistory.xaml
    public partial class UserTicketHistory : Page
    {
        public UserTicketHistory()
        {
            InitializeComponent();

            IDataAccessService dataAccess = IoCContainer.GetService<IDataAccessService>();
            IUserService userService = IoCContainer.GetService<IUserService>();

            DataContext = new UserTicketHistoryVM(dataAccess, userService);
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
