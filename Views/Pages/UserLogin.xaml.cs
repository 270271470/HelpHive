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
using HelpHive.ViewModels;
using HelpHive.Commands;
using HelpHive.Models;
using HelpHive.Services;
using HelpHive.Utilities;
using HelpHive.DataAccess;
using System.Diagnostics;

namespace HelpHive.Views.Pages
{
    /// <summary>
    /// Interaction logic for UserLogin.xaml
    /// </summary>
    public partial class UserLogin : Page
    {
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).Password = ((PasswordBox)sender).Password;
            }
        }

        public UserLogin()
        {
            InitializeComponent();
            DataContext = IoCContainer.GetService<UserLoginVM>();
        }
    }
}