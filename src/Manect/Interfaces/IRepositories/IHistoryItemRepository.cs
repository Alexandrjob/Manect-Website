using System.Collections.Generic;
using System.Threading.Tasks;
using Manect.Controllers.Models;

namespace Manect.Interfaces.IRepositories
{
    public interface IHistoryItemRepository
    {
        Task<List<HistoryItem>> GetHistoryAsync();
    }
}