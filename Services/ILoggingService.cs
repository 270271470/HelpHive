using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpHive.Services
{
    public interface ILoggingService
    {
        void Log(string message, LogLevel level);
    }

    public enum LogLevel
    {
        Info,
        Warning,
        Error
    }
}