using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noexia.MyAI.Inferences.Core.Logs
{
    public static class LogsManager
    {
        public static event Action<LogMessage> OnLogMessage;

        public static void Log(string title, string message, ELogMessage type)
        {
            OnLogMessage?.Invoke(new LogMessage(title, message, type));
        }

        public static void Log(string title, string message)
        {
            OnLogMessage?.Invoke(new LogMessage(title, message));
        }

        public static void Log(string message, ELogMessage type)
        {
            OnLogMessage?.Invoke(new LogMessage(message, type));
        }

        public static void Log(string message)
        {
            OnLogMessage?.Invoke(new LogMessage(message));
        }

        public static void LogWarning(string title, string message)
        {
            OnLogMessage?.Invoke(new LogMessage(title, message, ELogMessage.Warning));
        }

        public static void LogWarning(string message)
        {
            OnLogMessage?.Invoke(new LogMessage(message, ELogMessage.Warning));
        }

        public static void LogError(string title, string message)
        {
            OnLogMessage?.Invoke(new LogMessage(title, message, ELogMessage.Error));
        }

        public static void LogError(string message)
        {
            OnLogMessage?.Invoke(new LogMessage(message, ELogMessage.Error));
        }

        public static void RegisterToConsole()
        {
            OnLogMessage += (log) =>
            {
                Console.WriteLine(log.ToString());
            };
        }

        public static void RegisterToDebug()
        {
            OnLogMessage += (log) =>
            {
                System.Diagnostics.Debug.WriteLine(log.ToString());
            };
        }
    }
}
