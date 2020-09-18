using Microsoft.Extensions.Logging;

namespace Manect.HistoryLogger
{
    public static class DataBaseLoggerExtensions
    {
        public static ILoggerFactory AddDataBase(this ILoggerFactory factory,
                                        string filePath)
        {
            factory.AddProvider(new DataBaseLoggerProvider(filePath));
            return factory;
        }
    }
}
