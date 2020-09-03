using System.Threading.Tasks;

namespace Manect.Interfaces
{
    interface IRepository<T> where T : class
    {
        
        Task<T> FindByEmailAsync(string email);
        Task<T> ToListAsync();
    }
}
