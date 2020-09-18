using Manect.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manect.Interfaces
{
    public interface IDataRepository 
    {
        Task<ExecutorUser> FindUserByNameOrDefaultAsync(string name);
        Task<ExecutorUser> FindUserByEmailOrDefaultAsync(string email);
        Task<Project> FindProjectAsync(string name);
        Task<Stage> AddStageAsync(ExecutorUser user, Project project);
        Task DeleteStageAsync(Stage stage);
        Task DeleteProjectAsync(Project project);
        Task<Project> AddProjectDefaultAsync(ExecutorUser user);
        //TODO: Это не очень хорошо - делать выборку по имени, в будущем при необходимости переделать
        Task<List<Project>> ToListProjectOrDefaultAsync(string userName);
    }
}
