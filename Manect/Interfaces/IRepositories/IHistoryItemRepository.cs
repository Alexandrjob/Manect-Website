using Manect.Controllers.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manect.Interfaces.IRepositories
{
    public interface IHistoryItemRepository
    {
        Task<List<HistoryItem>> GetHistoryAsync();
    }
}
