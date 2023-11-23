using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace HelpHive.Services
{
    // default connection string to connect to remote mysql database
    public class DatabaseConnection
    {
        private readonly string _connectionString;

        public DatabaseConnection(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public MySqlConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public bool TestConnection()
        {
            try
            {
                using (MySqlConnection connection = CreateConnection())
                {
                    connection.Open();  // This will throw an exception if there's an issue connecting
                    connection.Close();
                    return true;        // Returns true if connection is successful
                }
            }
            catch
            {
                return false;           // Returns false if exception occurs
            }
        }

    }
}