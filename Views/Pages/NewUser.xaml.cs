﻿using HelpHive.Services;
using HelpHive.Utilities;
using HelpHive.ViewModels;
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
    /// Interaction logic for NewUser.xaml
    public partial class NewUser : Page
    {

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).User.Password = ((PasswordBox)sender).Password;
            }
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).ConfirmPassword = ((PasswordBox)sender).Password;
            }
        }

        public NewUser()
        {
            InitializeComponent();

            ILoggingService logginService = IoCContainer.GetService<ILoggingService>();
            INavigationService navigationService = IoCContainer.GetService<INavigationService>();

            DataContext = new UserVM(logginService, navigationService);
        }
    }
}
