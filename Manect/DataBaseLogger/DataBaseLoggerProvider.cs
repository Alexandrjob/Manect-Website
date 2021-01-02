using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Manect.DataBaseLogger
{
    public class DataBaseLoggerProvider: ILoggerProvider
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public DataBaseLoggerProvider(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new DbLogger(_serviceScopeFactory);
        }

        public void Dispose()
        {
        }
    }
}
