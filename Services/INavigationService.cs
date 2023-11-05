using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// This interface defines the methods for navigating between pages

namespace HelpHive.Services
{
    public interface INavigationService
    {
        void NavigateTo(string pageKey);
        void GoBack();
    }
}