using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using HelpHive.Views;
using HelpHive.ViewModels;
using HelpHive.ViewModels.Pages;
using HelpHive.Services;
using HelpHive.DataAccess;

namespace HelpHive.Utilities
{
    public static class IoCContainer
    {
        public static ServiceProvider ServiceProvider { get; private set; }

        public static void Setup()
        {
            var services = new ServiceCollection();

            // Register services with their interfaces
            services.AddSingleton<IAdminService, AdminService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<INavigationService, AppNavigationService>();
            services.AddSingleton<IDataAccessService, DataAccessLayer>();
            services.AddSingleton<ITicketService, TicketService>();

            // Register ViewModels
            services.AddTransient<MainWindowVM>();
            services.AddTransient<WelcomePageVM>();
            services.AddTransient<UserLoginVM>();
            services.AddTransient<UserDashVM>();
            services.AddTransient<UserNewTicketVM>();
            services.AddTransient<UserTicketRepliesVM>();
            services.AddTransient<AdminVM>();
            services.AddTransient<AdminLoginVM>();
            services.AddTransient<AdminDashVM>();
            services.AddTransient<AdminTicketRepliesVM>();

            ServiceProvider = services.BuildServiceProvider();
        }

        public static T GetService<T>()
        {
            return ServiceProvider.GetService<T>();
        }
    }
}
