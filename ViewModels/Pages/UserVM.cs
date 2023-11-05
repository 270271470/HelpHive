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

        // This command will be bound to the Register button in the view
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
            Debug.WriteLine($"isValidEmail: {isValidEmail}");

            bool isValidPhoneNumber = string.IsNullOrWhiteSpace(User?.PhoneNumber) ? false
                                    : Regex.IsMatch(User.PhoneNumber, @"^(\+\d{1,3}[- ]?)?\d{10}$", RegexOptions.IgnoreCase);
            Debug.WriteLine($"isValidPhoneNumber: {isValidPhoneNumber}");

            bool hasFirstName = !string.IsNullOrWhiteSpace(User.FirstName);
            Debug.WriteLine($"hasFirstName: {hasFirstName}");

            bool hasLastName = !string.IsNullOrWhiteSpace(User.LastName);
            Debug.WriteLine($"hasLastName: {hasLastName}");

            bool hasEmail = !string.IsNullOrWhiteSpace(User.Email);
            Debug.WriteLine($"hasEmail: {hasEmail}");

            bool hasValidPassword = !string.IsNullOrWhiteSpace(User.Password);
            Debug.WriteLine($"hasValidPassword: {hasValidPassword}");

            bool passwordMatchesConfirmation = User.Password == _confirmPassword; // Assuming you have a _confirmPassword field in your ViewModel
            Debug.WriteLine($"passwordMatchesConfirmation: {passwordMatchesConfirmation}");

            bool hasAddress1 = !string.IsNullOrWhiteSpace(User.Address1);
            Debug.WriteLine($"hasAddress1: {hasAddress1}");

            bool hasCity = !string.IsNullOrWhiteSpace(User.City);
            Debug.WriteLine($"hasCity: {hasCity}");

            bool hasRegion = !string.IsNullOrWhiteSpace(User.Region);
            Debug.WriteLine($"hasRegion: {hasRegion}");

            bool hasPostalCode = !string.IsNullOrWhiteSpace(User.PostalCode);
            Debug.WriteLine($"hasPostalCode: {hasPostalCode}");

            bool hasCountry = !string.IsNullOrWhiteSpace(User.Country);
            Debug.WriteLine($"hasCountry: {hasCountry}");

            bool hasStatus = !string.IsNullOrWhiteSpace(User.Status);
            Debug.WriteLine($"hasStatus: {hasStatus}");

            // Assuming country should be a valid two-letter country code.
            bool isValidCountryCode = !string.IsNullOrWhiteSpace(User.Country) && Regex.IsMatch(User.Country, @"^[A-Z]{2}$");
            Debug.WriteLine($"isValidCountryCode: {isValidCountryCode}");

            bool canRegister = hasFirstName;

            Debug.WriteLine($"canRegister result: {canRegister}");

            return canRegister;
        }

        private void Register(object parameter)
        {
            Debug.WriteLine("Register method called");
            try
            {
                // Hash the password
                User.Password = HashPassword(User.Password);

                Debug.WriteLine("Password hash called");
                // Set DateCreated to the current date/time - Check added above.
                //User.DateCreated = DateTime.Now;


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
