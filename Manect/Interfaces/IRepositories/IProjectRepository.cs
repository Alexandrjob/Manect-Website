using Manect.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manect.Interfaces.IRepositories
{
    public interface IProjectRepository
    {
        Task AddProjectDefaultAsync(int CurrentUserId);
        Task DeleteProjectAsync(int userId, int projectId);
        Task<Project> GetProjectAllDataAsync(int projectId);
        Task<Project> GetProjectInfoAsync(int projectId);
        Task<List<Project>> GetProjectOrDefaultToListAsync(int userId);
        Task EditProjectAsync(Project project, int CurrentUserId);
    }
}
