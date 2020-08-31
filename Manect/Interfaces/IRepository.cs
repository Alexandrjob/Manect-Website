using System.Threading.Tasks;

namespace Manect.Interfaces
{
    interface IRepository<T> where T : class
    {
        //T FindById(int id);
        Task<T> FindByEmailAsync(string email);

    }
}
