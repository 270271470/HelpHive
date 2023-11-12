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
    /// Interaction logic for UserNewTicket.xaml
    public partial class UserNewTicket : Page
    {

        public UserNewTicket()
        {
            InitializeComponent();

            IDataAccessService dataAccess = IoCContainer.GetService<IDataAccessService>();
            IUserService userService = IoCContainer.GetService<IUserService>();
            ITicketService ticketService = IoCContainer.GetService<ITicketService>();
            INavigationService navigationService = IoCContainer.GetService<INavigationService>();

            DataContext = new UserNewTicketVM(dataAccess, userService,ticketService,navigationService);
        }
    }
}
