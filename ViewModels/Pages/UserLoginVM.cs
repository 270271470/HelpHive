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
    public class UserLoginVM : ViewModelBaseClass
    {
        private readonly INavigationService _navigationService;
        private readonly IDataAccessService _dataAccess;
        private readonly IUserService _userService;

        // Constructor for UserLoginVM
        public UserLoginVM(INavigationService navigationService, IDataAccessService dataAccess, IUserService userService)
        {
            _navigationService = navigationService;
            _dataAccess = dataAccess;
            _userService = userService;

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

        private void Login(object parameter)
        {
            try
            {
                var hashedPassword = HashPassword(_password);
                var user = _dataAccess.VerifyUser(_email, hashedPassword);

                if (user != null)
                {
                    _navigationService.NavigateTo("UserDash");
                    
                    // Login method after successful authentication
                    _userService.Login(user); // Pass 'user' to login method
                }
                else
                {
                    MessageBox.Show("Invalid email or password", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


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