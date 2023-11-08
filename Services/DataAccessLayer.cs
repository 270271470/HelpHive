using HelpHive.Models;
using HelpHive.Services;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Diagnostics;

namespace HelpHive.DataAccess
{
    public class DataAccessLayer : IDataAccessService // Implementing the interface
    {
        private string _connectionString;

        public DataAccessLayer()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        // VerifyUser before logging in
        public UserModel VerifyUser(string email, string hashedPassword)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM tblusers WHERE email = @Email AND password = @Password LIMIT 1";

                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", hashedPassword);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Map the data to the UserModel or a custom UserModel class
                                var user = new UserModel();
                                // Set properties on user from reader
                                return user;
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine("MySQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error occurred: " + ex.Message);
            }
            return null; // Or throw exception, or handle accordingly
        }


        //Registering a New User
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
