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
using System.Collections.ObjectModel;

namespace HelpHive.ViewModels.Pages
{
    public class AdminVM : ViewModelBaseClass
    {
        private AdminModel _admin;
        private DataAccessLayer _dataAccess;
        private readonly ILoggingService _loggingService;
        private readonly INavigationService _navigationService;

        public RelayCommand NavigateToAdminDashCommand { get; private set; }

        // Constructor for AdminVM, init DAL + default admin model values
        public AdminVM(IDataAccessService dataAccess, IUserService userService, ITicketService ticketService, INavigationService navigationService, ILoggingService loggingService)
        {
            _loggingService = loggingService;
            _navigationService = navigationService;

            NavigateToAdminDashCommand = new RelayCommand(ExecuteNavigateToAdminDash);

            _dataAccess = new DataAccessLayer();

            _admin= new AdminModel
            {
                DateCreated = DateTime.Now  // Default date created set to current date and time
            };

            //Admin roles from DB
            // Init the ObservableCollection
            AdminRoles = new ObservableCollection<AdminRolesModel>();
            // Load departments from the database
            LoadAdminRoles();

            //Departments from DB
            // Init the ObservableCollection
            Departments = new ObservableCollection<TicketDeptsModel>();
            // Load departments from the database
            LoadDepartments();

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
                Admin.Password == _confirmPassword;

            // Return result of validation
            return canRegister;
        }

        private void ExecuteNavigateToAdminDash(object parameter)
        {
            _navigationService.NavigateTo("AdminDash");
        }

        // Method to handle admin registration
        private void Register(object parameter)
        {
            Debug.WriteLine("Register Admin method called");
            try
            {

                // Check if admin email is already registered
                if (_dataAccess.IsAdminEmailRegistered(Admin.Email))
                {
                    MessageBox.Show("Email already exists in our database. Please use a different email, or Log in.", "Email Exists", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return; // Stop the registration process
                }
                // Hash users pass before registering
                Admin.Password = HashPassword(Admin.Password);

                Admin.RoleId = SelectedAdminRoleId;
                Admin.Departments = SelectedDepartmentName;


                // Try to register user using DAL
                var success = _dataAccess.RegisterAdmin(Admin);
                if (success)
                {
                    MessageBox.Show("Admin registered successfully!");
                    _loggingService.Log($"ADMIN - New Admin Sccount created - {Admin.Email}", LogLevel.Info);
                    _navigationService.NavigateTo("AdminLogin");
                }
                else
                {
                    MessageBox.Show("Registration failed. Please try again.");
                    _loggingService.Log($"ADMIN - Admin Account creation failed.", LogLevel.Error);
                }
            }
            catch (Exception ex)
            {
                // If error, log the exception details
                Debug.WriteLine($"Registration failed: {ex.Message}");
                _loggingService.Log($"ADMIN - Admin Account creation failed - {ex.Message}", LogLevel.Error);
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

        //Populate the AdminRoles collection from DB
        private void LoadAdminRoles()
        {
            var adminroleList = _dataAccess.GetAdminRoles(); // Calling GetAdminRoles
            foreach (var role in adminroleList)
            {
                AdminRoles.Add(role);
            }
        }



        // Collection of AdminRoles prop to hold the selected role's ID
        public ObservableCollection<AdminRolesModel> AdminRoles { get; set; }

        private int _selectedAdminRoleId;
        public int SelectedAdminRoleId
        {
            get => _selectedAdminRoleId;
            set
            {
                if (_selectedAdminRoleId != value)
                {
                    _selectedAdminRoleId = value;
                    OnPropertyChanged(nameof(SelectedAdminRoleId));
                    // Re-evaluate the CanExecute of the command
                    RegisterCommand.RaiseCanExecuteChanged();
                }
            }
        }



        //Populate the Departments collection from DB
        private void LoadDepartments()
        {
            var departmentList = _dataAccess.GetDepartments(); // Calling GetDepartments
            foreach (var dept in departmentList)
            {
                Departments.Add(dept);
            }
        }


        // Collection of Dept prop to hold the selected department's ID
        public ObservableCollection<TicketDeptsModel> Departments { get; set; }

        private string _selectedDepartmentName;
        public string SelectedDepartmentName
        {
            get => _selectedDepartmentName;
            set
            {
                if (_selectedDepartmentName != value)
                {
                    _selectedDepartmentName = value;
                    OnPropertyChanged(nameof(SelectedDepartmentName));
                    // Re-evaluate the CanExecute of the command
                    RegisterCommand.RaiseCanExecuteChanged();
                }
            }
        }

    }
}