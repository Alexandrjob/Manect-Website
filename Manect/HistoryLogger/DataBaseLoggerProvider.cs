using Microsoft.Extensions.Logging;

namespace Manect.HistoryLogger
{
    public class DataBaseLoggerProvider: ILoggerProvider
    {
        private string path;
        public DataBaseLoggerProvider(string _path)
        {
            path = _path;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new DbLogger(path);
        }

        public void Dispose()
        {
        }
    }
}
