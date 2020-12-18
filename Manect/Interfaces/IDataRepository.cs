using Manect.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manect.Interfaces
{
    public interface IDataRepository
    {
        Task<Executor> FindUserIdByNameOrDefaultAsync(string name);
        Task AddStageAsync(int userId, int projectId);
        Task AddProjectDefaultAsync(Executor user);
        Task DeleteStageAsync(int userId, int projectId, int stageId);
        Task DeleteProjectAsync(int userId, int projectId);
        Task SetFlagValueAsync(int userId, int projectId, int stageId, Status status);
        Task<Project> GetAllProjectDataAsync(int projectId, int stageId = default);
        Task<List<Project>> GetProjectOrDefaultToListAsync(int userId);
        Task<List<Executor>> GetExecutorsToListExceptAsync(int executorId);
        Task ChengeExecutorAsync(int executorId, int projectId, int stageId);
        Task ChangeStageAsync(Stage stage);
        Task ChangeProjectAsync(Project project, int userId);
        Task AddFileAsync(DataToChange dataToChange);
        Task DeleteFileAsync(DataToChange dataToChange);
        Task<AppFile> GetFileAsync(DataToChange dataToChange);
        Task<List<AppFile>> FileListAsync(DataToChange dataToChange);
        Task<List<Executor>> GetProgectListExecutorsAsync();
        Task<bool> IsAdminAsync(DataToChange dataToChange);
    }
}
