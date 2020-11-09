using Manect.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manect.Interfaces
{
    public interface IDataRepository
    {
        Task<Executor> FindUserIdByNameOrDefaultAsync(string name);
        Task<Executor> FindUserByEmailOrDefaultAsync(string email);
        Task<Stage> AddStageAsync(int userId, int projectId);
        Task<Project> AddProjectDefaultAsync(Executor user);
        Task DeleteStageAsync(int userId, int projectId, int stageId);
        Task DeleteProjectAsync(int userId, int projectId);
        Task SetFlagValueAsync(int userId, int projectId, int stageId, Status status);
        Task<Project> GetAllProjectDataAsync(int projectId, int stageId = default);
        //TODO: В будущем при необходимости переделать это не очень хорошо - делать выборку по имени.
        Task<List<Project>> GetProjectOrDefaultToListAsync(int userId);
        Task<List<Executor>> GetExecutorsToListExceptAsync(int executorId);
        Task ChengeExecutorAsync(int executorId, int projectId, int stageId);
        Task ChangeStageAsync(Stage stage);
        Task ChangeProjectAsync(Project project);
    }
}
