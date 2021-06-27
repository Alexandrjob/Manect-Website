using Manect.Controllers.Models;
using System.Threading.Tasks;

namespace Manect.Interfaces
{
    public abstract class StringFormat
    {
        public abstract bool Contains(HistoryItem item);

        public abstract string Execute(HistoryItem item);
    }
}
