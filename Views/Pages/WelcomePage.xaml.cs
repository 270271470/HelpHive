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

            /*
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var dbConnection = new DatabaseConnection(connectionString);

            bool isConnected = dbConnection.TestConnection();

            if (isConnected)
            {
                MessageBox.Show("Connected to the database successfully!");
            }
            else
            {
                MessageBox.Show("Failed to connect to the database.");
            }*/

        }
    }
}
