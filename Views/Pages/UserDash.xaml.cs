using HelpHive.Services;
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
    /// <summary>
    /// Interaction logic for UserDash.xaml
    /// </summary>
    public partial class UserDash : Page
    {
        public UserDash()
        {
            InitializeComponent();

            // Retrieve the services from the App class
            IDataAccessService dataAccess = App.DataAccessService;
            IUserService userService = App.UserService;

            // Set the DataContext for the UserDash view
            DataContext = new UserDashVM(dataAccess, userService);
        }
    }
}