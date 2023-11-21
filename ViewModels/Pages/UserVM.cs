using HelpHive.Commands;
using HelpHive.Models;
using HelpHive.Services;
using HelpHive.DataAccess;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Security.Cryptography;     // For hashing
using System.Text;
using System.Windows;
using System.Text.RegularExpressions;   // Regex validation
using System.Diagnostics;

namespace HelpHive.ViewModels
{
    public class UserVM : ViewModelBaseClass
    {
        // Private fields to hold user data and data access logic
        private UserModel _user;
        private DataAccessLayer _dataAccess;
        private readonly INavigationService _navigationService;
        private readonly ILoggingService _loggingService;

        // Validation message properties
        public string FirstNameValidationMessage { get; private set; } = string.Empty;
        public string LastNameValidationMessage { get; private set; } = string.Empty;
        public string EmailValidationMessage { get; private set; } = string.Empty;
        public string PasswordValidationMessage { get; private set; } = string.Empty;
        public string ConfirmPasswordValidationMessage { get; private set; } = string.Empty;
        public string PhoneNumberValidationMessage { get; private set; } = string.Empty;
        public string Address1ValidationMessage { get; private set; } = string.Empty;
        public string CityValidationMessage { get; private set; } = string.Empty;
        public string RegionValidationMessage { get; private set; } = string.Empty;
        public string PostalCodeValidationMessage { get; private set; } = string.Empty;
        public string CountryValidationMessage { get; private set; } = string.Empty;

        public RelayCommand NavigateToUserDashCommand { get; private set; }

        // Constructor for UserVM
        public UserVM(ILoggingService loggingService, INavigationService navigationService)
        {
            _loggingService = loggingService;
            _navigationService = navigationService;

            NavigateToUserDashCommand = new RelayCommand(ExecuteNavigateToUserDash);

            _dataAccess = new DataAccessLayer();
            _user = new UserModel
            {
                Status = "Active",
                DateCreated = DateTime.Now
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
                    RegisterCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public RelayCommand RegisterCommand { get; private set; }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }

        private bool CanRegister(object parameter)
        {
            ResetValidationMessages();

            ValidateFirstName();
            ValidateLastName();
            ValidateEmail();
            ValidatePassword();
            ValidateConfirmPassword();
            ValidatePhoneNumber();
            ValidateAddress1();
            ValidateCity();
            ValidateRegion();
            ValidatePostalCode();
            ValidateCountry();

            bool canRegister = string.IsNullOrEmpty(FirstNameValidationMessage)
                                && string.IsNullOrEmpty(LastNameValidationMessage)
                                && string.IsNullOrEmpty(EmailValidationMessage)
                                && string.IsNullOrEmpty(PasswordValidationMessage)
                                && string.IsNullOrEmpty(ConfirmPasswordValidationMessage)
                                && string.IsNullOrEmpty(PhoneNumberValidationMessage)
                                && string.IsNullOrEmpty(Address1ValidationMessage)
                                && string.IsNullOrEmpty(CityValidationMessage)
                                && string.IsNullOrEmpty(RegionValidationMessage)
                                && string.IsNullOrEmpty(PostalCodeValidationMessage)
                                && string.IsNullOrEmpty(CountryValidationMessage);

            return canRegister;
        }

        private void ExecuteNavigateToUserDash(object parameter)
        {
            _navigationService.NavigateTo("UserDash");
        }

        // Method to handle user registration
        private void Register(object parameter)
        {
            Debug.WriteLine("Register method called");
            try
            {
                // Check if user email is already registered
                if (_dataAccess.IsUserEmailRegistered(User.Email))
                {
                    MessageBox.Show("Email already exists in our database. Please use a different email, or Log in.", "Email Exists", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return; // Stop the registration process
                }

                // Hash users pass before registering
                User.Password = HashPassword(User.Password);

                // Try to register user using DAL
                var success = _dataAccess.RegisterUser(User);
                if (success)
                {
                    MessageBox.Show("User registered successfully!");
                    _loggingService.Log($"USER - New account created - {User.Email}.", LogLevel.Info);
                    _navigationService.NavigateTo("UserLogin");
                }
                else
                {
                    MessageBox.Show("Registration failed. Please try again.");
                    _loggingService.Log($"USER - New account creation failed.", LogLevel.Error);
                }
            }
            catch (Exception ex)
            {
                // If error, log the exception details
                Debug.WriteLine($"Registration failed: {ex.Message}");
                _loggingService.Log($"USER - New account creation failed.", LogLevel.Error);
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

        // Validation methods
        private void ResetValidationMessages()
        {
            FirstNameValidationMessage = string.Empty;
            LastNameValidationMessage = string.Empty;
            EmailValidationMessage = string.Empty;
            PasswordValidationMessage = string.Empty;
            ConfirmPasswordValidationMessage = string.Empty;
            PhoneNumberValidationMessage = string.Empty;
            Address1ValidationMessage = string.Empty;
            CityValidationMessage = string.Empty;
            RegionValidationMessage = string.Empty;
            PostalCodeValidationMessage = string.Empty;
            CountryValidationMessage = string.Empty;
        }

        private void ValidateFirstName()
        {
            if (string.IsNullOrWhiteSpace(User?.FirstName))
            {
                FirstNameValidationMessage = "Required";
                OnPropertyChanged(nameof(FirstNameValidationMessage));
            }
        }

        private void ValidateLastName()
        {
            if (string.IsNullOrWhiteSpace(User?.LastName))
            {
                LastNameValidationMessage = "Required";
                OnPropertyChanged(nameof(LastNameValidationMessage));
            }
        }

        private void ValidateEmail()
        {
            if (string.IsNullOrWhiteSpace(User?.Email))
            {
                EmailValidationMessage = "Required";
            }
            else if (!Regex.IsMatch(User.Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", RegexOptions.IgnoreCase))
            {
                EmailValidationMessage = "Invalid email format.";
            }
            OnPropertyChanged(nameof(EmailValidationMessage));
        }

        private void ValidatePassword()
        {
            if (string.IsNullOrWhiteSpace(User?.Password))
            {
                PasswordValidationMessage = "Required";
            }
            OnPropertyChanged(nameof(PasswordValidationMessage));
        }

        private void ValidateConfirmPassword()
        {
            if (User.Password != _confirmPassword)
            {
                ConfirmPasswordValidationMessage = "Passwords do not match.";
            }
            OnPropertyChanged(nameof(ConfirmPasswordValidationMessage));
        }

        private void ValidatePhoneNumber()
        {
            if (string.IsNullOrWhiteSpace(User?.PhoneNumber))
            {
                PhoneNumberValidationMessage = "Required";
            }
            else if (!Regex.IsMatch(User.PhoneNumber, @"^(\+\d{1,3}[- ]?)?\d{10}$", RegexOptions.IgnoreCase))
            {
                PhoneNumberValidationMessage = "Invalid phone number format.";
            }
            OnPropertyChanged(nameof(PhoneNumberValidationMessage));
        }

        private void ValidateAddress1()
        {
            if (string.IsNullOrWhiteSpace(User?.Address1))
            {
                Address1ValidationMessage = "Required";
                OnPropertyChanged(nameof(Address1ValidationMessage));
            }
        }

        private void ValidateCity()
        {
            if (string.IsNullOrWhiteSpace(User?.City))
            {
                CityValidationMessage = "Required";
                OnPropertyChanged(nameof(CityValidationMessage));
            }
        }

        private void ValidateRegion()
        {
            if (string.IsNullOrWhiteSpace(User?.Region))
            {
                RegionValidationMessage = "Required";
                OnPropertyChanged(nameof(RegionValidationMessage));
            }
        }

        private void ValidatePostalCode()
        {
            if (string.IsNullOrWhiteSpace(User?.PostalCode))
            {
                PostalCodeValidationMessage = "Required";
                OnPropertyChanged(nameof(PostalCodeValidationMessage));
            }
        }

        private void ValidateCountry()
        {
            if (string.IsNullOrWhiteSpace(User?.Country))
            {
                CountryValidationMessage = "Required";
            }
            else if (!Regex.IsMatch(User.Country, @"^[A-Z]{2}$"))
            {
                CountryValidationMessage = "Invalid country format. Use 2-letter country code.";
            }
            OnPropertyChanged(nameof(CountryValidationMessage));
        }

    }
}