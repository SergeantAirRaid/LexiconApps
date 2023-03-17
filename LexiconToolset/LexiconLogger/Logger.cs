using System;
using System.Collections.Generic;
using System.IO;
using Extensions;

namespace LexiconLogger
{
    public class Logger
    {
        public string LogFile { get; private set; }

        public LoggerSeverity MinSeverity { get; set; }

        public enum LoggerSeverity
        {
            Error,
            Warning,
            Info,
            Debug
        }

        public Logger(LoggerSeverity severity = LoggerSeverity.Info) 
            : this("LexiconLogger_" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + ".csv", severity) { }

        public Logger(string logFile, LoggerSeverity severity = LoggerSeverity.Info)
        {
            try
            {
                File.AppendAllLines(logFile, new List<string>() { "Starting new log at " + DateTime.Now.ToLoggerText() });
            }
            catch { throw; }

            MinSeverity = severity;
            LogFile = logFile;
        }

        public void LogItem(string message, LoggerSeverity severity)
        {
            if (!((int)severity <= (int)MinSeverity)) return;
            string entry = DateTime.Now.ToLoggerText() + "," + severity.ToString().ToUpper() + ",\"" + message + "\"";
            File.AppendAllLines(LogFile, new List<string>() { entry });
        }

        public void LogItems(List<string> messages, LoggerSeverity severity)
        {
            foreach (var message in messages)
            {
                LogItem(message, severity);
            }
        }

        public void LogError(string message)
        {
            LogItem(message, LoggerSeverity.Error);
        }

        public void LogWarning(string message)
        {
            LogItem(message, LoggerSeverity.Warning);
        }

        public void LogInfo(string message)
        {
            LogItem(message, LoggerSeverity.Info);
        }

        public void LogDebug(string message)
        {
            LogItem(message, LoggerSeverity.Debug);
        }

        public void Spacer()
        {
            File.AppendAllLines(LogFile, new List<string>() { "" });
        }
    }
}

namespace Extensions
{
    public static class ExtensionMethods
    {
        public static string ToLoggerText(this DateTime time)
        {
            return time.Month + "/" + time.Day + "/" + time.Year + " " + time.Hour + ":" + time.Minute + ":" + time.Second;
        }
    }
}
