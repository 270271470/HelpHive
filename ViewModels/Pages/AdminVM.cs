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
using System.Text.RegularExpressions;   // Regex validation - 13/11/23 - Mauritz
using System.Diagnostics;

namespace HelpHive.ViewModels.Pages
{
    public class AdminVM : ViewModelBaseClass
    {
        private AdminModel _admin;
        private DataAccessLayer _dataAccess;

        // Constructor for AdminVM, init DAL + default admin model values
        public AdminVM()
        {
            _dataAccess = new DataAccessLayer();
            _admin= new AdminModel
            {
                DateCreated = DateTime.Now  // Default date created set to current date and time
            };

            // Init RegisterCommand with actions to execute and conditions when to be executable
            RegisterCommand = new RelayCommand(Register, CanRegister);
        }

        // Public prop to get and set the UserModel. Raises property changed notifications
        public AdminModel Admin
        {
            get => _admin;
            set
            {
                if (_admin != value)
                {
                    _admin = value;
                    OnPropertyChanged(nameof(Admin));
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
                !string.IsNullOrWhiteSpace(Admin?.FirstName) &&
                !string.IsNullOrWhiteSpace(Admin?.LastName) &&
                !string.IsNullOrWhiteSpace(Admin?.UserName) &&
                !string.IsNullOrWhiteSpace(Admin?.Email) && Regex.IsMatch(Admin.Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", RegexOptions.IgnoreCase) &&
                !string.IsNullOrWhiteSpace(Admin?.Password) &&
                Admin.Password == _confirmPassword &&
                !string.IsNullOrWhiteSpace(Admin?.Departments);

            // Return result of validation
            return canRegister;
        }

        // Method to handle admin registration
        private void Register(object parameter)
        {
            Debug.WriteLine("Register Admin method called");
            try
            {
                // Hash users pass before registering
                Admin.Password = HashPassword(Admin.Password);

                // Try to register user using DAL
                var success = _dataAccess.RegisterAdmin(Admin);
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