using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Manect.DataBaseLogger
{
    public class DataBaseLoggerProvider: ILoggerProvider
    {
        private string path;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public DataBaseLoggerProvider(string _path, IServiceScopeFactory serviceScopeFactory)
        {
            path = _path;
            _serviceScopeFactory = serviceScopeFactory;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new DbLogger(path, _serviceScopeFactory);
        }

        public void Dispose()
        {
        }
    }
}
