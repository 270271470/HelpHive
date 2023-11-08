using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpHive.Views.Pages;
using HelpHive.Utilities;
using HelpHive.Services;
using HelpHive.ViewModels.Pages;
using System.Windows.Controls;

// This interface defines the methods for navigating between pages

namespace HelpHive.Services
{
    public interface INavigationService
    {
        void NavigateTo(string pageKey);
        void GoBack();
        void SetMainFrame(Frame frame);
    }
}