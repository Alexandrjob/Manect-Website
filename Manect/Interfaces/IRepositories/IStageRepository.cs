using Manect.Controllers.Models;
using Manect.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manect.Interfaces.IRepositories
{
    public interface IStageRepository
    {
        Task AddStageAsync(int userId, int projectId);
        Task DeleteStageAsync(int userId, int projectId, int stageId);
        Task SetFlagValueAsync(int userId, int projectId, int stageId, Status status);
        Task<Stage> GetStageAsync(int stageId);
        Task EditStageAsync(Stage stage);
        Task<List<Project>> GetStagesListDelegatedAsync(DataToChange dataToChange);
    }
}
