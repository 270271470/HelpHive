using HelpHive.Models;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Diagnostics;

namespace HelpHive.DataAccess
{
    public class DataAccessLayer
    {
        private string _connectionString;

        public DataAccessLayer()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public bool RegisterUser(UserModel user)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    string sql = @"INSERT INTO tblusers (firstname, lastname, companyname, email, password, address1, address2, city, region, postalcode, country, phonenumber, status, datecreated)
                            VALUES (@FirstName, @LastName, @CompanyName, @Email, @Password, @Address1, @Address2, @City, @Region, @PostalCode, @Country, @PhoneNumber, 'Active', CURRENT_TIMESTAMP)";

                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", user.FirstName ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@LastName", user.LastName ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@CompanyName", user.CompanyName ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Email", user.Email ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Password", user.Password ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Address1", user.Address1 ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Address2", user.Address2 ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@City", user.City ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Region", user.Region ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@PostalCode", user.PostalCode ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Country", user.Country ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber ?? (object)DBNull.Value);

                        command.ExecuteNonQuery();
                    }
                    return true;
                }
            }
            catch (MySqlException mySqlEx)
            {
                // Log the MySQL exception
                Debug.WriteLine("MySQL Error: " + mySqlEx.Message);
                return false;
            }
            catch (Exception ex)
            {
                // Log general exceptions
                Debug.WriteLine("An error occurred: " + ex.Message);
                return false;
            }
        }
    }
}
