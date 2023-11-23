using HelpHive.Commands;
using HelpHive.Models;
using HelpHive.Services;
using HelpHive.DataAccess;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;
using System.Text.RegularExpressions;   // Included for Regex validation
using System.Diagnostics;
using System.Windows;

namespace HelpHive.ViewModels
{
    public class AdminLoginVM : ViewModelBaseClass
    {
        private readonly INavigationService _navigationService;
        private readonly IDataAccessService _dataAccess;
        private readonly IAdminService _adminService;
        private readonly ILoggingService _loggingService;

        // Constructor for AdminLoginVM
        public AdminLoginVM(INavigationService navigationService, IDataAccessService dataAccess, IAdminService adminService, ILoggingService loggingService)
        {
            _navigationService = navigationService;
            _dataAccess = dataAccess;
            _adminService = adminService;
            _loggingService = loggingService;

            // Sets up LoginCommand with both the execute (Login) & can-execute (CanLogin).
            LoginCommand = new RelayCommand(Login, CanLogin);
        }

        // Email info received from the form
        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
                LoginCommand.RaiseCanExecuteChanged();
                Debug.WriteLine("Email is " + _email);
            }
        }

        // Passwd info received from the form
        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                LoginCommand.RaiseCanExecuteChanged();
                Debug.WriteLine("Password " + _password);
            }
        }

        // Command prop to bind to Login Button
        public RelayCommand LoginCommand { get; }

        // Method to determine if LoginCommand can exe based on info from user
        private bool CanLogin(object parameter)
        {
            Debug.WriteLine("Check if we can login - Trap 1");

            // Validation logic to check completeness and correctness of user information
            bool canLogin =
                !string.IsNullOrWhiteSpace(_email) && Regex.IsMatch(_email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", RegexOptions.IgnoreCase) &&
                !string.IsNullOrWhiteSpace(_password);

            // Return result of the validation
            Debug.WriteLine("CanLogin: " + canLogin);
            return canLogin;
        }

        // login method
        private void Login(object parameter)
        {
            try
            {
                var hashedPassword = HashPassword(_password);
                var admin = _dataAccess.VerifyAdmin(_email, hashedPassword);

                if (admin != null)
                {
                    _navigationService.NavigateTo("AdminDash");
                    _loggingService.Log($"ADMIN - Successful login from {Email}", LogLevel.Info);
                    // Login method after successful authentication
                    _adminService.Login(admin); // Pass 'admin' to login method
                }
                else
                {
                    MessageBox.Show("Invalid email or password", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    _loggingService.Log($"ADMIN - Invalid login attempt from {Email}", LogLevel.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // handling the hashed passwd
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}