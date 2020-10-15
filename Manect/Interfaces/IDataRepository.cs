using Manect.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manect.Interfaces
{
    public interface IDataRepository
    {
        Task<int> FindUserIdByNameOrDefaultAsync(string name);
        Task<Executor> FindUserByEmailOrDefaultAsync(string email);
        Task<Project> FindProjectAsync(string name);
        Task<Stage> AddStageAsync(int userId, int projectId);
        Task<Project> AddProjectDefaultAsync(int userId);
        Task DeleteStageAsync(int userId, int projectId, int stageId);
        Task DeleteProjectAsync(int userId, int projectId);
        Task SetFlagValueAsync(int userId, int projectId, int stageId, Status status);
        Task<Project> GetAllProjectDataAsync(int projectId);
        //TODO: В будущем при необходимости переделать это не очень хорошо - делать выборку по имени.
        Task<List<Project>> ToListProjectOrDefaultAsync(int userId);
    }
}
