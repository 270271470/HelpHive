using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using HelpHive.Utilities;
using HelpHive.Views;

namespace HelpHive
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            IoCContainer.Setup(); // Set up the DI container

            var mainWindow = new MainWindow(); // Assuming MainWindow is your entry point
            mainWindow.Show();
        }
    }
}
