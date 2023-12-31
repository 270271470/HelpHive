﻿using System;
using System.Windows.Controls;
using HelpHive.Views.Pages;
using HelpHive.Utilities;
using HelpHive.Services;
using HelpHive.ViewModels.Pages;

namespace HelpHive.Services
{
    // this is the navigation service - responsible for handling all navigation between pages
    // we register each page in the NavigateTo function and use a siwtch case to hand the actual navigation
    public class AppNavigationService : INavigationService
    {
        private Frame _frame;

        public AppNavigationService()
        {
        }

        // Method to set main frame from MainWindow
        public void SetMainFrame(Frame frame)
        {
            _frame = frame ?? throw new ArgumentNullException(nameof(frame));
        }

        public void NavigateTo(string pageKey)
        {
            switch (pageKey)
            {
                case "NewUser":
                    _frame.Navigate(new Uri("/Views/Pages/NewUser.xaml", UriKind.Relative));
                    break;
                case "NewAdmin":
                    _frame.Navigate(new Uri("/Views/Pages/NewAdmin.xaml", UriKind.Relative));
                    break;
                case "UserLogin":
                    _frame.Navigate(new Uri("/Views/Pages/UserLogin.xaml", UriKind.Relative));
                    break;
                case "AdminLogin":
                    _frame.Navigate(new Uri("/Views/Pages/AdminLogin.xaml", UriKind.Relative));
                    break;
                case "UserDash":
                    _frame.Navigate(new Uri("/Views/Pages/UserDash.xaml", UriKind.Relative));
                    break;
                case "AdminDash":
                    _frame.Navigate(new Uri("/Views/Pages/AdminDash.xaml", UriKind.Relative));
                    break;
                // Add cases for other pages
                default:
                    throw new ArgumentException("Unknown page key", nameof(pageKey));
            }
        }

        public void GoBack()
        {
            if (_frame.CanGoBack)
            {
                _frame.GoBack();
            }
        }
    }
}
