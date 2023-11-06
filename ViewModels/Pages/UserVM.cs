// Namespace inclusions for various classes and functions used in the code.
using HelpHive.Commands;
using HelpHive.Models;
using HelpHive.Services;
using HelpHive.DataAccess;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Text.RegularExpressions;   // Included for Regex validation
using System.Diagnostics;

// Namespace declaration for the ViewModel.
namespace HelpHive.ViewModels
{
    // Define UserVM class that inherits from ViewModelBaseClass following the MVVM pattern.
    public class UserVM : ViewModelBaseClass
    {
        // Private fields to hold user data and data access logic.
        private UserModel _user;
        private DataAccessLayer _dataAccess;

        // Constructor for UserVM, initializes data access layer and default user model values.
        public UserVM()
        {
            _dataAccess = new DataAccessLayer();
            _user = new UserModel
            {
                Status = "Active", // Default status for a new user is set to 'Active'.
                DateCreated = DateTime.Now // Default date created is set to the current date and time.
            };

            // Initialize the RegisterCommand with actions to execute and conditions when to be executable.
            RegisterCommand = new RelayCommand(Register, CanRegister);
        }

        // Public property to get and set the UserModel. Raises property changed notifications.
        public UserModel User
        {
            get => _user;
            set
            {
                if (_user != value)
                {
                    _user = value;
                    OnPropertyChanged(nameof(User));
                    // Notify that the RegisterCommand may need to reevaluate its executable status.
                    RegisterCommand.RaiseCanExecuteChanged();
                }
            }
        }

        // Command property to be bound to a registration button or action in the view.
        public RelayCommand RegisterCommand { get; private set; }

        // Field to store the confirmation of the user's password.
        private string _confirmPassword;
        // Property to get and set the confirmed password with notification.
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
                // Notify that the RegisterCommand may need to reevaluate its executable status.
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }

        // Method to determine if the RegisterCommand can execute based on the current state of the UserModel properties.
        private bool CanRegister(object parameter)
        {
            // Validation logic to check the completeness and correctness of user information.
            // Email, phone number, and country code are validated using regex. Other fields are checked for non-emptiness.
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

            // Return the result of the validation.
            return canRegister;
        }

        // Method to handle user registration.
        private void Register(object parameter)
        {
            Debug.WriteLine("Register method called");
            try
            {
                // Hash the user's password before registering.
                User.Password = HashPassword(User.Password);

                // Try to register the user using the data access layer.
                var success = _dataAccess.RegisterUser(User);
                if (success)
                {
                    MessageBox.Show("User registered successfully!");
                    // Further actions after successful registration can be added here.
                }
                else
                {
                    MessageBox.Show("Registration failed. Please try again.");
                }
            }
            catch (Exception ex)
            {
                // In case of an error, log the exception details.
                Debug.WriteLine($"Registration failed: {ex.Message}");
            }
        }

        // Method to hash a password using SHA256.
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                // Convert the password to a byte array and compute the hash.
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                // Convert the byte array to a hex string.
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // The ViewModelBaseClass should implement INotifyPropertyChanged.
        // An example of OnPropertyChanged method is commented out below.
        /*
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        */
    }
}