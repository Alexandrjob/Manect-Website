using Manect.Controllers.Models;

namespace Manect.Interfaces
{
    public abstract class StringFormat
    {
        public abstract bool Contains(HistoryItem item);

        public abstract string Execute(HistoryItem item);
    }
}