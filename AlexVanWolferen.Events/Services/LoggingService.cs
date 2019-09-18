using AlexVanWolferen.Events.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlexVanWolferen.Events.Services
{
    public class LoggingService : ILoggingService
    {
        public void Fatal(Type type, string message)
        {
            Sitecore.Diagnostics.Log.Fatal(message, type);
        }

        public void Error(Type type, string message)
        {
            Sitecore.Diagnostics.Log.Error(message, type);
        }

        public void Error(Type type, string message, Exception ex)
        {
            Sitecore.Diagnostics.Log.Error(message, ex, type);
        }

        public void Warning(Type type, string message)
        {
            Sitecore.Diagnostics.Log.Warn(message, type);
        }

        public void Info(Type type, string message)
        {
            Sitecore.Diagnostics.Log.Info(message, type);
        }

        public void Debug(Type type, string message)
        {
            Sitecore.Diagnostics.Log.Debug(message, type);
        }
    }
}