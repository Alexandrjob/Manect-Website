using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;

namespace Manect.HistoryLogger
{
    public class DbLogger : ILogger
    {
        /// <summary>
        /// Путь к файлу.
        /// </summary>
        private string filePath;
        private static object _lock = new object();
        public DbLogger(string path)
        {
            filePath = path;
            var uName = "Sasha";
            var pName = "Шкаф";
            var pTime = DateTime.Now;
            var item = new HistoryItem()
            {
                ExecutorName = uName,
                ProjectName = pName,
                TimeAction = pTime

            };
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == LogLevel.Information;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                lock (_lock)
                {
                    if (state is string)
                    {
                        var StateText = state.ToString();
                    }

                    else if (state is IEnumerable<KeyValuePair<string, object>> Properties)
                    {
                        var StateProperties = new Dictionary<string, object>();

                        foreach (KeyValuePair<string, object> item in Properties)
                        {
                            StateProperties[item.Key] = item.Value;
                        }
                    }

                    File.AppendAllText(filePath, formatter(state, exception) + Environment.NewLine);
                }
            }
        }
    }
}
