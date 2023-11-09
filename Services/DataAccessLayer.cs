﻿using HelpHive.Models;
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




        //Create A New Ticket
        public bool CreateNewTicket(TicketModel ticket)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    string sql = @"INSERT INTO tbltickets (tid, did, uid, name, email, date, title, message, ticketstatus, incidentstatus, urgency, admin, attachment, attachments_removed,lastreply,clientunread,adminread,replytime)
                            VALUES (@TicketId, @DeptId, @UserId, @Name, @Email, CURRENT_TIMESTAMP, @Message, @TicketStatus @IncidentStatus, @Urgency, @Admin, @Attachment, @AttachmentsRemoved, CURRENT_TIMESTAMP, @ClientUnread, @AdminRead, CURRENT_TIMESTAMP)";

                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@TicketId", ticket.TicketId);
                        command.Parameters.AddWithValue("@DeptId", ticket.DeptId);
                        command.Parameters.AddWithValue("@UserId", ticket.UserId);
                        command.Parameters.AddWithValue("@Name", ticket.Name ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Email", ticket.Email ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Message", ticket.Message ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TicketStatus", ticket.TicketStatus ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@IncidentStatus", ticket.IncidentStatus ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Urgency", ticket.Urgency ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Admin", ticket.Admin ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Attachment", ticket.Attachment ?? (object)DBNull.Value);

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
                                // Map the data to the UserModel
                                var user = new UserModel();
                                // Set properties on user from reader
                                {
                                    //user.UserId = reader["uid"].ToString();
                                    user.UserId = reader["uid"] != DBNull.Value ? Convert.ToInt32(reader["uid"]) : default(int);
                                    user.FirstName = reader["firstname"].ToString();
                                    user.LastName = reader["lastname"].ToString();
                                    user.CompanyName = reader["companyname"].ToString();
                                    user.Email = reader["email"].ToString();
                                    user.Address1 = reader["address1"].ToString();
                                    user.Address2 = reader["address2"].ToString();
                                    user.City = reader["city"].ToString();
                                    user.Region = reader["region"].ToString();
                                    user.PostalCode = reader["postalcode"].ToString();
                                    user.Country = reader["country"].ToString();
                                    user.PhoneNumber = reader["phonenumber"].ToString();
                                    user.Status = reader["Status"].ToString();
                                }
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

        public UserModel GetUserDetails(string email)
        {
            UserModel user = null;
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM tblusers WHERE email = @Email";
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new UserModel
                                {
                                    // UserModel class with properties that map to your database columns
                                    UserId = reader["uid"] != DBNull.Value ? Convert.ToInt32(reader["uid"]) : default(int),
                                    FirstName = reader["firstname"].ToString(),
                                    LastName = reader["lastname"].ToString(),
                                    CompanyName = reader["companyname"].ToString(),
                                    Email = reader["email"].ToString(),
                                    Address1 = reader["address1"].ToString(),
                                    Address2 = reader["address2"].ToString(),
                                    City = reader["city"].ToString(),
                                    Region = reader["region"].ToString(),
                                    PostalCode = reader["postalcode"].ToString(),
                                    Country = reader["country"].ToString(),
                                    PhoneNumber = reader["phonenumber"].ToString(),
                                    Status = reader["Status"].ToString(),
                            };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions, possibly logging them
                Debug.WriteLine("An error occurred: " + ex.Message);
            }
            return user;
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
