using HelpHive.DataAccess;
using System;
using HelpHive.Models;
using System.Collections.Generic;
using System.Data;

namespace HelpHive.Services
{
    // the logging service - responsible to handling the logging of system events
    public class LoggingService : ILoggingService
    {
        private readonly IDataAccessService _dataAccessService;

        public LoggingService(IDataAccessService dataAccessService)
        {
            _dataAccessService = dataAccessService;
        }

        public void Log(string message, LogLevel level)
        {
            // logic to add log entry to database.
            try
            {
                var logEntry = new LogEntry
                {
                    Message = message,
                    Level = level.ToString(),
                    Timestamp = DateTime.Now
                };

                _dataAccessService.AddLogEntry(logEntry);
            }
            catch (Exception ex)
            {
                // might output these exceptions to a file if time permits.
            }
        }
    }

    public class LogEntry
    {
        public string Message { get; set; }
        public string Level { get; set; }
        public DateTime Timestamp { get; set; }
    }
}