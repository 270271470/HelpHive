using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using HelpHive.DataAccess;
using HelpHive.Services;
using HelpHive.Utilities;
using HelpHive.Views;

namespace HelpHive
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Static properties to hold the services
        public static IDataAccessService DataAccessService { get; private set; }
        public static IUserService UserService { get; private set; }


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Initialize your services here
            DataAccessService = new DataAccessLayer();
            UserService = new UserService();

            IoCContainer.Setup(); // Set up the DI container

            var mainWindow = new MainWindow(); // Assuming MainWindow is your entry point
            mainWindow.Show();
        }
    }
}
