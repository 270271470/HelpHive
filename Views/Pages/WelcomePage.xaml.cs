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
    /// <summary>
    /// Interaction logic for WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Page
    {
        public WelcomePage()
        {
            InitializeComponent();

            // Incorrect instantiation if the constructor expects no arguments
            //var navigationService = new AppNavigationService(NavigationService);

            // Correct instantiation if using method injection
            var navigationService = new AppNavigationService();
            // Assuming IoCContainer is already configured and can resolve IAppNavigationService
            // var navigationService = IoCContainer.GetService<IAppNavigationService>();

            DataContext = new WelcomePageVM(navigationService);

        }
    }
}
