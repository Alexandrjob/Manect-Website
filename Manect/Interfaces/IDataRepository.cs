using Manect.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manect.Interfaces
{
    public interface IDataRepository 
    {
        //TODO: Создать метод, который находит 1 проект(мы еще должны понять какой это проект).
        //TODO: Создать метод, который удаляет 1 этап.
        Task<ExecutorUser> FindUserByNameOrDefaultAsync(string name);
        Task<ExecutorUser> FindUserByEmailAsync(string email);
        Task AddStageAsync(ExecutorUser user, Project project);
        Task DeleteStageAsync(Stage stage);
        Task AddProjectDefaultAsync(ExecutorUser user);
        //TODO: Это не очень хорошо - делать проверку по имени, в будущем при необходимости переделать
        Task<List<Project>> ToListProjectsAsync(string userName);
    }
}
