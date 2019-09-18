namespace AlexVanWolferen.Events.Services.Interfaces
{
    using System;

    public interface ILoggingService
    {
        void Fatal(Type type, string message);

        void Error(Type type, string message);

        void Error(Type type, string message, Exception ex);

        void Warning(Type type, string message);

        void Info(Type type, string message);

        void Debug(Type type, string message);
    }
}