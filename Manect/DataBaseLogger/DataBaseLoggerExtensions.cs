using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Manect.DataBaseLogger
{
    public static class DataBaseLoggerExtensions
    {
        public static ILoggerFactory AddDataBase(this ILoggerFactory factory, IServiceScopeFactory serviceScopeFactory)
        {
            factory.AddProvider(new DataBaseLoggerProvider(serviceScopeFactory));
            return factory;
        }
    }
}
