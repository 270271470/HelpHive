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
using System.Text.RegularExpressions;   //Need for Regex validation - Mauritz 05/11/23
using System.Diagnostics;

namespace HelpHive.ViewModels
{
    public class UserVM : ViewModelBaseClass
    {
        private UserModel _user;
        private DataAccessLayer _dataAccess;

        public UserVM()
        {
            _dataAccess = new DataAccessLayer();
            _user = new UserModel
            {
                // Initialize default values for new properties
                Status = "Active", // Set the default status
                DateCreated = DateTime.Now // Set the default date created to now
            };

            RegisterCommand = new RelayCommand(Register, CanRegister);
        }

        public UserModel User
        {
            get => _user;
            set
            {
                if (_user != value)
                {
                    _user = value;
                    OnPropertyChanged(nameof(User));
                    // Update the CanRegister command's CanExecute state
                    RegisterCommand.RaiseCanExecuteChanged();
                }
            }
        }

        // This command will be bound to the Register button in the vie
        public RelayCommand RegisterCommand { get; }


        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
                RegisterCommand.RaiseCanExecuteChanged(); // This will refresh the CanExecute state of the RegisterCommand
            }
        }

        private bool CanRegister(object parameter)
        {
            bool isValidEmail = string.IsNullOrWhiteSpace(User?.Email) ? false
                                : Regex.IsMatch(User.Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", RegexOptions.IgnoreCase);
            bool isValidPhoneNumber = string.IsNullOrWhiteSpace(User?.PhoneNumber) ? false
                                    : Regex.IsMatch(User.PhoneNumber, @"^(\+\d{1,3}[- ]?)?\d{10}$", RegexOptions.IgnoreCase);
            bool hasFirstName = !string.IsNullOrWhiteSpace(User.FirstName);
            bool hasLastName = !string.IsNullOrWhiteSpace(User.LastName);
            bool hasEmail = !string.IsNullOrWhiteSpace(User.Email);
            bool hasValidPassword = !string.IsNullOrWhiteSpace(User.Password);
            bool passwordMatchesConfirmation = User.Password == _confirmPassword;
            bool hasAddress1 = !string.IsNullOrWhiteSpace(User.Address1);
            bool hasCity = !string.IsNullOrWhiteSpace(User.City);
            bool hasRegion = !string.IsNullOrWhiteSpace(User.Region);
            bool hasPostalCode = !string.IsNullOrWhiteSpace(User.PostalCode);
            bool hasCountry = !string.IsNullOrWhiteSpace(User.Country);
            bool hasStatus = !string.IsNullOrWhiteSpace(User.Status);

            // Country code must be in this format - NZ, AU etc.
            bool isValidCountryCode = !string.IsNullOrWhiteSpace(User.Country) && Regex.IsMatch(User.Country, @"^[A-Z]{2}$");

            bool canRegister = hasFirstName && hasLastName && hasEmail && hasValidPassword  && passwordMatchesConfirmation  && isValidPhoneNumber && hasAddress1 && hasCity && hasRegion && hasPostalCode && hasCountry;

            //Debug.WriteLine($"canRegister result: {canRegister}");

            return canRegister;
        }

        private void Register(object parameter)
        {
            Debug.WriteLine("Register method called");
            try
            {
                // Hash the password
                User.Password = HashPassword(User.Password);

                //Debug.WriteLine("Password hash called");

                // Attempt to register the user
                var success = _dataAccess.RegisterUser(User);
                if (success)
                {
                    MessageBox.Show("User registered successfully!");
                    // Optionally, navigate away or clear the form
                }
                else
                {
                    MessageBox.Show("Registration failed. Please try again.");
                }
            }
            catch (Exception ex)
            {
                // Log exception (consider using a logging framework)
                //MessageBox.Show("An error occurred during registration. Please try again.");
                Debug.WriteLine($"Registration failed: {ex.Message}");
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // The ViewModelBaseClass should already implement INotifyPropertyChanged
        // Make sure it includes a method like the one below to raise the PropertyChanged event
        /*
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        */
    }
}
