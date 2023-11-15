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
    /// Interaction logic for App.xaml
    public partial class App : Application
    {
        // Static props to hold services
        public static IDataAccessService DataAccessService { get; private set; }
        public static IUserService UserService { get; private set; }
        public static IAdminService AdminService { get; private set; }


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Init services
            DataAccessService = new DataAccessLayer();
            UserService = new UserService();
            AdminService = new AdminService();

            // DI container
            IoCContainer.Setup();

            // MainWindow is our entry point
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
