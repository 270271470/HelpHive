// ViewModels/UserLoginVM.cs
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

// Namespace declaration for the ViewModel.
namespace HelpHive.ViewModels
{
    // Define UserLoginVM class that inherits from ViewModelBaseClass following the MVVM pattern.
    public class UserLoginVM : ViewModelBaseClass
    {
        // Private fields to hold data access logic.
        private readonly INavigationService _navigationService;
        private readonly DataAccessLayer _dataAccess;

        // Constructor for UserLoginVM, initializes nav for redirect, data access layer
        public UserLoginVM(INavigationService navigationService, DataAccessLayer dataAccess)
        {
            _navigationService = navigationService;
            _dataAccess = dataAccess;
            //LoginCommand = new RelayCommand(Login);
            // Initialize the LoginCommand with actions to execute and conditions when to be executable.
            LoginCommand = new RelayCommand(Login, CanLogin);
        }

        //This was added by VS Built-in Help on 07/11/23
        //Constructor
        public UserLoginVM()
        {
        }

        //Email Info received from the form
        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
                Debug.WriteLine("Email is " + _email);
            }
        }

        //Password Info received from the form
        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                Debug.WriteLine("Password " + _password);
            }
        }

        //Removing this for now
        //public ICommand LoginCommand { get; }

        // Command property to be bound to Login Button or action in the view.
        public RelayCommand LoginCommand { get; }

        // Method to determine if the LoginCommand can execute based on info provided by the user
        private bool CanLogin(object parameter)
        {
            Debug.WriteLine("Check if we can login - Trap 1");
            // Validation logic to check the completeness and correctness of user information.
            // Email, phone number, and country code are validated using regex. Other fields are checked for non-emptiness.
            bool canLogin =
                !string.IsNullOrWhiteSpace(_email) && Regex.IsMatch(_email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", RegexOptions.IgnoreCase) &&
                !string.IsNullOrWhiteSpace(_password);

            // Return the result of the validation.
            Debug.WriteLine("CanLogin: " + canLogin);
            return canLogin;
        }

        private void Login(object parameter)
        {
            var hashedPassword = HashPassword(_password);
            var user = _dataAccess.VerifyUser(_email, hashedPassword);

            if (user != null)
            {
                // Assuming the VerifyUser method returns a user object on successful authentication.
                _navigationService.NavigateTo("UserDash");
            }
            else
            {
                // Show an error message or handle login failure
                Debug.WriteLine("Cannot Login");
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