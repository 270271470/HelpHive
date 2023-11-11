using HelpHive.Commands;
using HelpHive.Models;
using HelpHive.Services;
using HelpHive.DataAccess;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Security.Cryptography;     // For hasing
using System.Text;
using System.Windows;
using System.Text.RegularExpressions;   // Regex validation - 06/11/23 - Mauritz
using System.Diagnostics;

namespace HelpHive.ViewModels
{
    public class UserVM : ViewModelBaseClass
    {
        // Private fields to hold user data and data access logic
        private UserModel _user;
        private DataAccessLayer _dataAccess;

        // Constructor for UserVM, init DAL + default user model values
        public UserVM()
        {
            _dataAccess = new DataAccessLayer();
            _user = new UserModel
            {
                Status = "Active",          // Default status for new user set to Active
                DateCreated = DateTime.Now  // Default date created set to current date and time
            };

            // Init RegisterCommand with actions to execute and conditions when to be executable
            RegisterCommand = new RelayCommand(Register, CanRegister);
        }

        // Public prop to get and set the UserModel. Raises property changed notifications
        public UserModel User
        {
            get => _user;
            set
            {
                if (_user != value)
                {
                    _user = value;
                    OnPropertyChanged(nameof(User));
                    // Notify that RegisterCommand may need to reevaluate its executable status
                    RegisterCommand.RaiseCanExecuteChanged();
                }
            }
        }

        // Command property to be bound to a registration button or action in the view
        public RelayCommand RegisterCommand { get; private set; }

        // Field to store the confirmation of the user's password
        private string _confirmPassword;
        // Property to get and set the confirmed password with notification
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
                // Notify that the RegisterCommand may need to reevaluate its executable status
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }

        // Method to determine if RegisterCommand can execute based on the current state of the UserModel props
        private bool CanRegister(object parameter)
        {
            // Validation logic to check completeness and correctness of user information
            // Email, phone number, and country code are validated using regex. Other fields are checked for non-emptiness
            bool canRegister =
                !string.IsNullOrWhiteSpace(User?.FirstName) &&
                !string.IsNullOrWhiteSpace(User?.LastName) &&
                !string.IsNullOrWhiteSpace(User?.Email) && Regex.IsMatch(User.Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", RegexOptions.IgnoreCase) &&
                !string.IsNullOrWhiteSpace(User?.Password) &&
                User.Password == _confirmPassword &&
                !string.IsNullOrWhiteSpace(User?.PhoneNumber) && Regex.IsMatch(User.PhoneNumber, @"^(\+\d{1,3}[- ]?)?\d{10}$", RegexOptions.IgnoreCase) &&
                !string.IsNullOrWhiteSpace(User?.Address1) &&
                !string.IsNullOrWhiteSpace(User?.City) &&
                !string.IsNullOrWhiteSpace(User?.Region) &&
                !string.IsNullOrWhiteSpace(User?.PostalCode) &&
                !string.IsNullOrWhiteSpace(User?.Country) &&
                Regex.IsMatch(User.Country, @"^[A-Z]{2}$");

            // Return result of validation
            return canRegister;
        }

        // Method to handle user registration
        private void Register(object parameter)
        {
            Debug.WriteLine("Register method called");
            try
            {
                // Hash users pass before registering
                User.Password = HashPassword(User.Password);

                // Try to register user using DAL
                var success = _dataAccess.RegisterUser(User);
                if (success)
                {
                    MessageBox.Show("User registered successfully!");
                    // Maybe add redirect here to UserDash
                }
                else
                {
                    MessageBox.Show("Registration failed. Please try again.");
                }
            }
            catch (Exception ex)
            {
                // If error, log the exception details
                Debug.WriteLine($"Registration failed: {ex.Message}");
            }
        }

        // Method to hash a password using SHA256
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                // Convert the password to a byte array + compute the hash
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                // Convert the byte array to a hex string
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}