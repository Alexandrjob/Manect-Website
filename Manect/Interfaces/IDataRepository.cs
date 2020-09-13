using Manect.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manect.Interfaces
{
    public interface IDataRepository 
    {
        Task<ExecutorUser> FindUserByNameOrDefaultAsync(string name);
        Task<ExecutorUser> FindUserByEmailAsync(string email);
        Task AddStageAsync(ExecutorUser user, Project project);
        Task AddProjectDefaultAsync(ExecutorUser user);
        Task<List<Project>> ToListProjectsAsync(ExecutorUser user);
    }
}
