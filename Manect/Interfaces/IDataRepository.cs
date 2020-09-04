using Manect.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manect.Interfaces
{
    public interface IDataRepository 
    {
        
        Task<ExecutorUser> FindByEmailAsync(string email);
        Task<List<Project>> ToListProjectsAsync(string userName);
    }
}
