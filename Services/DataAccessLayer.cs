using HelpHive.Models;
using HelpHive.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;

namespace HelpHive.DataAccess
{
    public class DataAccessLayer : IDataAccessService
    {
        private string _connectionString;

        public DataAccessLayer()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public List<TicketReplyModel> GetTicketReplies(string ticketId)
        {
            List<TicketReplyModel> replies = new List<TicketReplyModel>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string sql = @"SELECT tid, name, message, admin, email, rating, date 
                       FROM tblticketreplies 
                       WHERE tid = @TicketId 
                       ORDER BY date DESC";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@TicketId", ticketId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var reply = new TicketReplyModel
                            {
                                Tid = reader["tid"].ToString(),
                                Name = reader["name"].ToString(),
                                Email = reader["email"].ToString(),
                                Message = reader["message"].ToString(),
                                Admin = reader["admin"].ToString(),
                                Rating = reader.IsDBNull(reader.GetOrdinal("rating")) ? 0 : reader.GetInt32("rating"),
                                Date = reader.GetDateTime("date")
                            };
                            replies.Add(reply);
                        }
                    }
                }
            }

            return replies;
        }


        public TicketModel GetTicketDetails(string ticketId)
        {
            TicketModel ticket = null;

            using (var connection = new MySqlConnection(_connectionString))
            {
                // Open the connection
                connection.Open();

                // Create a command to execute the query
                using (var command = connection.CreateCommand())
                {
                    //command.CommandText = "SELECT * FROM tbltickets WHERE tid = @ticketId";
                    command.CommandText = "SELECT t.tid, t.did, t.uid, t.name, t.email, t.title, t.message, t.ticketstatus, t.incidentstatus , t.urgency, t.admin, t.lastreply, d.name AS DepartmentName FROM tbltickets AS t JOIN tblticketdepartments AS d ON t.did = d.id WHERE t.tid = @ticketId";
                    command.Parameters.AddWithValue("@ticketId", ticketId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ticket = new TicketModel
                            {
                                TicketId = reader["tid"].ToString(),
                                DeptId = reader.GetInt32("did"),
                                Name = reader["name"].ToString(),
                                Email = reader["email"].ToString(),
                                Title = reader["title"].ToString(),
                                Message = reader["message"].ToString(),
                                DepartmentName = reader["DepartmentName"].ToString(),
                                TicketStatus = reader["ticketstatus"].ToString(),
                                IncidentStatus = reader["incidentstatus"].ToString(),
                                Urgency = reader["urgency"].ToString(),
                                Admin = reader["admin"].ToString(),
                                LastReply = reader.GetDateTime("lastreply")
                            };
                        }
                    }
                }
            }

            return ticket;
        }




        //Getting UserOpenTickets from DB
        public List<TicketModel> GetUserOpenTickets(int userId)
        {
            var opentickets = new List<TicketModel>();
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    //string sql = "SELECT d.name AS Department, t.title AS Subject, t.ticketstatus AS Status, t.lastreply AS LastUpdate FROM tbltickets AS t JOIN tblticketdepartments AS d ON t.did = d.id WHERE t.uid = @UserId AND t.ticketstatus IN('Open', 'On Hold'); ";
                    //string sql = "SELECT tid, did, uid, title, ticketstatus, lastreply FROM tbltickets WHERE uid = @UserId AND ticketstatus IN('Open', 'On Hold')";
                    string sql = "SELECT t.tid, t.did, t.uid, t.title, t.ticketstatus, t.lastreply, d.name AS DepartmentName FROM tbltickets AS t JOIN tblticketdepartments AS d ON t.did = d.id WHERE t.uid = @UserId AND t.ticketstatus IN('Open', 'Answered', 'User Reply', 'On Hold', 'Answered') ORDER BY t.lastreply DESC";
                    using (var command = new MySqlCommand(sql, connection))
                    {

                        // Binding the userId parameter - This is NB for the correct filtering.
                        command.Parameters.AddWithValue("@UserId", userId);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var openticket = new TicketModel
                                {
                                    TicketId = reader.GetString("tid"),
                                    //DeptId = reader.GetInt32("did"),
                                    DepartmentName = reader["DepartmentName"].ToString(),
                                    UserId = reader.GetInt32("uid"),
                                    Title = reader.GetString("title"),
                                    TicketStatus = reader.GetString("ticketstatus"),
                                    LastReply = reader.GetDateTime("lastreply")
                                };
                                opentickets.Add(openticket);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error occurred: " + ex.Message);
            }
            return opentickets;
        }


        //Getting UserTicketHistory from DB
        public List<TicketModel> GetUserTicketHistory(int userId)
        {
            var tickets = new List<TicketModel>();
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    //string sql = "SELECT d.name AS Department, t.title AS Subject, t.ticketstatus AS Status, t.lastreply AS LastUpdate FROM tbltickets AS t JOIN tblticketdepartments AS d ON t.did = d.id WHERE t.uid = @UserId AND t.ticketstatus IN('Open', 'On Hold'); ";
                    //string sql = "SELECT tid, did, uid, title, ticketstatus, lastreply FROM tbltickets WHERE uid = @UserId AND ticketstatus IN('Open', 'On Hold')";
                    string sql = "SELECT t.tid, t.did, t.uid, t.title, t.ticketstatus, t.lastreply, d.name AS DepartmentName FROM tbltickets AS t JOIN tblticketdepartments AS d ON t.did = d.id WHERE t.uid = @UserId AND t.ticketstatus IN('Not Resolved', 'Resolved', 'Closed') ORDER BY t.lastreply DESC";
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        // Binding the userId parameter - This is NB for the correct filtering.
                        command.Parameters.AddWithValue("@UserId", userId);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var ticket = new TicketModel
                                {
                                    TicketId = reader.GetString("tid"),
                                    //DeptId = reader.GetInt32("did"),
                                    DepartmentName = reader["DepartmentName"].ToString(),
                                    UserId = reader.GetInt32("uid"),
                                    Title = reader.GetString("title"),
                                    TicketStatus = reader.GetString("ticketstatus"),
                                    LastReply = reader.GetDateTime("lastreply")
                                };
                                tickets.Add(ticket);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error occurred: " + ex.Message);
            }
            return tickets;
        }




        //GetOpenTicketsAsAdmin from DB
        public List<TicketModel> GetOpenTicketsAsAdmin()
        {
            var opentickets = new List<TicketModel>();
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                   
                    string sql = "SELECT t.tid, t.did, t.uid, t.name, t.title, t.ticketstatus, t.urgency, t.lastreply, d.name AS DepartmentName FROM tbltickets AS t JOIN tblticketdepartments AS d ON t.did = d.id WHERE t.ticketstatus IN('Open', 'User Reply', 'On Hold') ORDER BY t.lastreply DESC";
                    using (var command = new MySqlCommand(sql, connection))
                    {

                        // Binding the userId parameter - This is NB for the correct filtering.
                        //command.Parameters.AddWithValue("@UserId", userId);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var openticket = new TicketModel
                                {
                                    TicketId = reader.GetString("tid"),
                                    //DeptId = reader.GetInt32("did"),
                                    DepartmentName = reader["DepartmentName"].ToString(),
                                    UserId = reader.GetInt32("uid"),
                                    Name = reader.GetString("name"),
                                    Title = reader.GetString("title"),
                                    TicketStatus = reader.GetString("ticketstatus"),
                                    Urgency = reader.GetString("urgency"),
                                    LastReply = reader.GetDateTime("lastreply")
                                };
                                opentickets.Add(openticket);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error occurred: " + ex.Message);
            }
            return opentickets;
        }

        //GetAdminTicketHistory from DB
        public List<TicketModel> GetAdminTicketHistory()
        {
            var tickethistory = new List<TicketModel>();
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    //string sql = "SELECT t.tid, t.did, t.uid, t.name, t.title, t.ticketstatus, t.urgency, t.lastreply, d.name AS DepartmentName FROM tbltickets AS t JOIN tblticketdepartments AS d ON t.did = d.id WHERE t.ticketstatus IN('Open', 'Answered', 'User Reply', 'Resolved', 'Not Resolved', 'Closed', 'On Hold') ORDER BY date DESC";
                    string sql = "SELECT t.tid, t.did, t.uid, t.name, t.title, t.ticketstatus, t.urgency, t.lastreply, d.name AS DepartmentName FROM tbltickets AS t JOIN tblticketdepartments AS d ON t.did = d.id WHERE t.ticketstatus IN('Open', 'Answered', 'User Reply', 'Resolved', 'Not Resolved', 'Closed', 'On Hold') ORDER BY t.lastreply DESC";

                    using (var command = new MySqlCommand(sql, connection))
                    {

                        // Binding the userId parameter - This is NB for the correct filtering.
                        //command.Parameters.AddWithValue("@UserId", userId);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var alltickets = new TicketModel
                                {
                                    TicketId = reader.GetString("tid"),
                                    //DeptId = reader.GetInt32("did"),
                                    DepartmentName = reader["DepartmentName"].ToString(),
                                    UserId = reader.GetInt32("uid"),
                                    Name = reader.GetString("name"),
                                    Title = reader.GetString("title"),
                                    TicketStatus = reader.GetString("ticketstatus"),
                                    Urgency = reader.GetString("urgency"),
                                    LastReply = reader.GetDateTime("lastreply")
                                };
                                tickethistory.Add(alltickets);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error occurred: " + ex.Message);
            }
            return tickethistory;
        }






        //Getting Admin Roles from DB
        public List<AdminRolesModel> GetAdminRoles()
        {
            var adminroles = new List<AdminRolesModel>();
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM tbladminroles";
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var adminrole = new AdminRolesModel
                                {
                                    RoleId = reader.GetInt32(reader.GetOrdinal("id")),
                                    RoleName = reader["name"].ToString(),
                                };
                                adminroles.Add(adminrole);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error occurred: " + ex.Message);
            }
            return adminroles;
        }




        //Getting Admins from DB
        public List<AdminModel> GetAdmins()
        {
            var admins = new List<AdminModel>();
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM tbladmins";
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var admin = new AdminModel
                                {
                                    AdminId = reader.GetInt32(reader.GetOrdinal("aid")),
                                    RoleId = reader.GetInt32(reader.GetOrdinal("roleid")),
                                    UserName = reader["username"].ToString(),
                                    FirstName = reader["firstname"].ToString(),
                                    LastName = reader["lastname"].ToString(),
                                    Email= reader["email"].ToString(),
                                    Departments = reader["departments"].ToString(),
                                    TicketNotifications = reader["ticketnotifications"].ToString(),
                                };
                                admins.Add(admin);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error occurred: " + ex.Message);
            }
            return admins;
        }






        //Getting Departments from DB
        public List<TicketDeptsModel> GetDepartments()
        {
            var departments = new List<TicketDeptsModel>();
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM tblticketdepartments";
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var department = new TicketDeptsModel
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    Name = reader["name"].ToString(),
                                };
                                departments.Add(department);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error occurred: " + ex.Message);
            }
            return departments;
        }


        //Update Ticket Status
        public void UpdateTicketStatus(TicketModel ticket)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    string sql = @"UPDATE tbltickets SET ticketstatus = @TicketStatus, incidentstatus = @IncidentStatus WHERE tid = @TicketId";

                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@TicketStatus", ticket.TicketStatus);
                        command.Parameters.AddWithValue("@IncidentStatus", ticket.IncidentStatus);
                        command.Parameters.AddWithValue("@TicketId", ticket.TicketId);

                        command.ExecuteNonQuery();
                    }
                    //return true;
                }
            }
            catch (MySqlException mySqlEx)
            {
                // Log the MySQL exception
                Debug.WriteLine("MySQL Error: " + mySqlEx.Message);
                //return false;
            }
            catch (Exception ex)
            {
                // Log general exceptions
                Debug.WriteLine("An error occurred: " + ex.Message);
                //return false;
            }
        }



        //Create A New Ticket
        public bool CreateNewTicket(TicketModel ticket)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    string sql = @"INSERT INTO tbltickets (tid, did, uid, name, email, date, title, message, ticketstatus, incidentstatus, urgency, lastreply,clientunread,adminread,replytime)
                            VALUES (@TicketId, @DeptId, @UserId, @Name, @Email, @DateTime, @Title, @Message, 'Open', 'Not Resolved', @Urgency, @DateTime, 0, 0, @DateTime)";

                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@TicketId", ticket.TicketId);
                        command.Parameters.AddWithValue("@DeptId", ticket.DeptId);
                        command.Parameters.AddWithValue("@UserId", ticket.UserId);
                        command.Parameters.AddWithValue("@Name", ticket.Name ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Email", ticket.Email ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@DateTime", ticket.Date);
                        command.Parameters.AddWithValue("@Title", ticket.Title);
                        command.Parameters.AddWithValue("@Message", ticket.Message ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Urgency", ticket.Urgency ?? (object)DBNull.Value);

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

        //Admin Update Original Ticket
        public bool AdminOriginalUpdateTicket(TicketModel ticket)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    //string sql = @"INSERT INTO tbltickets (tid, did, ticketstatus, incidentstatus, urgency, admin, lastreply,)
                    //        VALUES (@TicketId, @DeptId, @TicketStatus, @IncidentStatus, @Urgency, @AdminName, @LastReply)";

                    string sql = @"UPDATE tbltickets SET tid=@TicketId, did=@DeptId, ticketstatus=@TicketStatus, incidentstatus=@IncidentStatus, urgency=@Urgency, admin=@AdminName, lastreply=@LastReply, replytime=@ReplyTime
                                WHERE tid=@TicketId";

                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@TicketId", ticket.TicketId);
                        command.Parameters.AddWithValue("@DeptId", ticket.DeptId);
                        command.Parameters.AddWithValue("@TicketStatus", ticket.TicketStatus);
                        command.Parameters.AddWithValue("@IncidentStatus", ticket.IncidentStatus);
                        command.Parameters.AddWithValue("@Urgency", ticket.Urgency ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@AdminName", ticket.Admin);
                        command.Parameters.AddWithValue("@LastReply", ticket.LastReply);
                        command.Parameters.AddWithValue("@ReplyTime", ticket.ReplyTime);
                        ;

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


        //User Update Original Ticket
        public bool UserOriginalUpdateTicket(TicketModel ticket)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    string sql = @"UPDATE tbltickets SET tid=@TicketId, ticketstatus=@TicketStatus, incidentstatus=@IncidentStatus, lastreply=@LastReply, replytime=@ReplyTime
                                WHERE tid=@TicketId";

                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@TicketId", ticket.TicketId);
                        command.Parameters.AddWithValue("@TicketStatus", ticket.TicketStatus);
                        command.Parameters.AddWithValue("@IncidentStatus", ticket.IncidentStatus);
                        command.Parameters.AddWithValue("@LastReply", ticket.LastReply);
                        command.Parameters.AddWithValue("@ReplyTime", ticket.ReplyTime);
                        ;

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


        //Insert Admin Ticket Reply
        public bool InsertAdminTicketReply(TicketReplyModel ticketReply)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    string sql = @"INSERT INTO tblticketreplies (tid, date, message, admin)
                            VALUES (@TicketId, @DateTime, @Message, @AdminName)";

                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@TicketId", ticketReply.Tid);
                        command.Parameters.AddWithValue("@DateTime", ticketReply.Date);
                        command.Parameters.AddWithValue("@Message", ticketReply.Message ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@AdminName", ticketReply.Admin ?? (object)DBNull.Value);
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

        //Insert User Ticket Reply
        public bool InsertUserTicketReply(TicketReplyModel ticketReply)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    string sql = @"INSERT INTO tblticketreplies (tid, uid, name, email, date, message, rating)
                            VALUES (@TicketId, @UserId, @Name, @Email, @DateTime, @Message, @Rating)";

                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@TicketId", ticketReply.Tid);
                        command.Parameters.AddWithValue("@UserId", ticketReply.UserId);
                        command.Parameters.AddWithValue("@Name", ticketReply.Name ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Email", ticketReply.Email ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@DateTime", ticketReply.Date);
                        command.Parameters.AddWithValue("@Message", ticketReply.Message ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Rating", ticketReply.Rating);
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
                                // Map data to the UserModel
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



        // VerifyAdmin before logging in
        public AdminModel VerifyAdmin(string email, string hashedPassword)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM tbladmins WHERE email = @Email AND password = @Password LIMIT 1";

                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", hashedPassword);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Map data to the UserModel
                                var admin = new AdminModel();
                                // Set properties on user from reader
                                {
                                    //user.UserId = reader["uid"].ToString();
                                    admin.AdminId = reader["aid"] != DBNull.Value ? Convert.ToInt32(reader["aid"]) : default(int);
                                    admin.RoleId = reader["roleid"] != DBNull.Value ? Convert.ToInt32(reader["roleid"]) : default(int);
                                    admin.UserName = reader["username"].ToString();
                                    admin.FirstName = reader["firstname"].ToString();
                                    admin.LastName = reader["lastname"].ToString();
                                    admin.Email = reader["email"].ToString();
                                    admin.Departments = reader["departments"].ToString();
                                    admin.TicketNotifications = reader["ticketnotifications"].ToString();
                                }
                                return admin;
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




        public AdminModel GetAdminDetails(string email)
        {
            AdminModel admin = null;
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM tbladmins WHERE email = @Email";
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                admin = new AdminModel
                                {
                                    // AdminModel class with properties that map to database columns
                                    AdminId = reader["aid"] != DBNull.Value ? Convert.ToInt32(reader["aid"]) : default(int),
                                    RoleId = reader["roleid"] != DBNull.Value ? Convert.ToInt32(reader["roleid"]) : default(int),
                                    UserName = reader["username"].ToString(),
                                    FirstName = reader["firstname"].ToString(),
                                    LastName = reader["lastname"].ToString(),
                                    Email = reader["email"].ToString(),
                                    Departments = reader["departments"].ToString(),
                                    TicketNotifications = reader["ticketnotifications"].ToString()
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
            return admin;
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

        //Registering a New Admin
        public bool RegisterAdmin(AdminModel admin)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    string sql = @"INSERT INTO tbladmins (roleid, username, password, firstname, lastname, email, departments, ticketnotifications, datecreated)
                            VALUES (@RoleId, @UserName, @Password, @FirstName, @LastName, @Email, @Departments, '0', CURRENT_TIMESTAMP)";

                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@RoleId", admin.RoleId);
                        command.Parameters.AddWithValue("@UserName", admin.UserName ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Password", admin.Password ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@FirstName", admin.FirstName ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@LastName", admin.LastName ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Email", admin.Email ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Departments", admin.Departments ?? (object)DBNull.Value);
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
