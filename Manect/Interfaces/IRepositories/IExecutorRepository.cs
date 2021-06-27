using Manect.Controllers.Models;
using Manect.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manect.Interfaces.IRepositories
{
    public interface IExecutorRepository
    {
        Task<int> FindUserIdByNameOrDefaultAsync(string name);
        Task<int> FindUserIdByEmailOrDefaultAsync(string email);
        Task<List<Executor>> GetExecutorsToListExceptAsync(int executorId);
        Task EditExecutorAsync(DataToChange dataToChange);
        Task<List<Executor>> GetProgectListExecutorsAsync();
        Task<bool> IsAdminAsync(DataToChange dataToChange);
    }
}
