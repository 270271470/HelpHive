using HelpHive.Services;
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
    /// Interaction logic for AdminDash.xaml
    public partial class AdminDash : Page
    {
        public AdminDash()
        {
            InitializeComponent();

            IDataAccessService dataAccess = IoCContainer.GetService<IDataAccessService>();
            IAdminService adminService = IoCContainer.GetService<IAdminService>();

            DataContext = new AdminDashVM(dataAccess, adminService);
        }

        // If admin clicks on a subject, this will redirect to the ticket replies page
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var grid = sender as DataGrid;
            var selectedTicket = grid.SelectedItem as TicketModel;
            if (selectedTicket != null)
            {
                //REMEMBER - Still need to change below to AdminTicketReplies.
                NavigationService.Navigate(new AdminTicketReplies(selectedTicket.TicketId));
            }
        }
    }
}
