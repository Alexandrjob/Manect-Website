using Manect.Controllers.Models;
using Manect.Data.Entities;
using ManectTelegramBot.Models;
using System.Threading.Tasks;

namespace Manect.Interfaces.IRepositories
{
    public interface ITelegramRepository
    {
        Task<long> GetTelegramIdAsync(DataToChange dataToChange);
        Task AddTelegramIdAsync(DataToChange dataToChange);
        Task<MessageObject> GetInformationForMessageAsync(DataToChange dataToChange);
        Task<Executor> GetFullNameAsync(DataToChange dataToChange);
    }
}
