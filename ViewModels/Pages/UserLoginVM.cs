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
using System.Windows;

// Namespace declaration for the ViewModel.
namespace HelpHive.ViewModels
{
    // Define UserLoginVM class that inherits from ViewModelBaseClass following the MVVM pattern.
    public class UserLoginVM : ViewModelBaseClass
    {
        // Private fields to hold data access logic.
        private readonly INavigationService _navigationService;
        private readonly IDataAccessService _dataAccess;
        private readonly IUserService _userService;

        // Constructor for UserLoginVM, initializes navigation service and data access service
        public UserLoginVM(INavigationService navigationService, IDataAccessService dataAccess, IUserService userService)
        {
            _navigationService = navigationService;
            _dataAccess = dataAccess;
            _userService = userService;

            LoginCommand = new RelayCommand(Login, CanLogin);
            //This sets up LoginCommand with both the execute delegate (Login) and the can-execute delegate (CanLogin).
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
                LoginCommand.RaiseCanExecuteChanged();
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
                LoginCommand.RaiseCanExecuteChanged();
                Debug.WriteLine("Password " + _password);
            }
        }

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
            try
            {
                var hashedPassword = HashPassword(_password);
                var user = _dataAccess.VerifyUser(_email, hashedPassword);

                if (user != null)
                {
                    // Inside the login method after successful authentication
                    _userService.Login(user); // Pass 'user' to the login method
                    _navigationService.NavigateTo("UserDash");
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