using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Manect.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Manect.DataBaseLogger
{
    public class DbLogger : ILogger
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private ProjectDbContext DataContext
        {
            get
            {
                var scope = _serviceScopeFactory.CreateScope();
                return scope.ServiceProvider.GetService<ProjectDbContext>();
            }
        }

        public DbLogger(IServiceScopeFactory serviceScopeFactory)
        {
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

        public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
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
                            if (item.Key.ToLower() == LogItemProperty[i].Name.ToLower())
                            {
                                LogItemProperty[i].SetValue(logItem, item.Value);
                                LogItemProperty = LogItemProperty.Where(e => LogItemProperty.ElementAt(i) != e)
                                    .ToArray();
                                break;
                            }
                        }
                    }

                    if (logItem.ExecutorId != default)
                    {
                        var dataContext = DataContext;
                        await dataContext.AddAsync(logItem);
                        await dataContext.SaveChangesAsync();
                    }
                }
            }
        }
    }
}