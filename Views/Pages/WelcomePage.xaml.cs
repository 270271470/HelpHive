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
using System.Configuration;
using HelpHive.Services;
using HelpHive.ViewModels.Pages;

// Code-behind - WelcomePage

namespace HelpHive.Views.Pages
{
    /// Interaction logic for WelcomePage.xaml
    public partial class WelcomePage : Page
    {
        public WelcomePage()
        {
            InitializeComponent();

            //Setting DataContext to WelcomePageVM
 
            var navigationService = new AppNavigationService();

            DataContext = new WelcomePageVM(navigationService);

        }
    }
}
