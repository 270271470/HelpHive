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
using HelpHive.Services;

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

            string connectionString = "Server=223.165.67.19;Database=helphive_db;User ID=helphive_admin;Password=qD7On3mDrwAr;SslMode=Required;";

            var dbConnection = new DatabaseConnection(connectionString);

            bool isConnected = dbConnection.TestConnection();

            if (isConnected)
            {
                MessageBox.Show("Connected to the database successfully!");
            }
            else
            {
                MessageBox.Show("Failed to connect to the database.");
            }
        }
    }
}
