using Manect.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Manect.DataBaseLogger
{
    public class DbLogger : ILogger
    {
        /// <summary>
        /// Путь к файлу.
        /// </summary>
        private string filePath;
        private static object _lock = new object();
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private ProjectDbContext DataContext
        {
            get
            {
                var scope = _serviceScopeFactory.CreateScope();
                return scope.ServiceProvider.GetService<ProjectDbContext>();
            }
        }

        public DbLogger(string path, IServiceScopeFactory serviceScopeFactory)
        {
            filePath = path;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == LogLevel.Information;
        }

        //TODO: Переименовать переменные для большей читабельости.
        public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null & IsEnabled(logLevel))
            {
                if (state is IEnumerable<KeyValuePair<string, object>> Properties)
                {
                    var logItem = new LogItem();

                    Type LogItemType = typeof(LogItem);
                    PropertyInfo[] LogItemProperty = LogItemType.GetProperties();

                    foreach (KeyValuePair<string, object> item in Properties)
                    {
                        for (int i = 0; i < LogItemProperty.Length; i++)
                        {
                            //Записать в свойство переменной logItem значение item.Value, если без учета регистра название item.Key равно имени свойства HistoryItem.
                            if (item.Key.ToLower() == LogItemProperty[i].Name.ToLower())
                            {
                                //Запись в logItem.
                                LogItemProperty[i].SetValue(logItem, item.Value);
                                //Типа оптимизирую(Создаю новый массив без элемента которое больше не понадобится).
                                LogItemProperty = LogItemProperty.Where(e => LogItemProperty.ElementAt(i) != e).ToArray();
                                break;
                            }
                        }
                    }
                    //TODO: Сделать адекватную проверку
                    if (logItem.ExecutorName != null)
                    {
                        var dataContext = DataContext;
                        await dataContext.AddAsync(logItem);
                        await dataContext.SaveChangesAsync();

                        lock (_lock)
                        {
                            //TODO: Реализовать запись в бд.
                            File.AppendAllText(filePath, formatter(state, exception) + Environment.NewLine);
                        }
                    }
                }
            }
        }
    }
}
