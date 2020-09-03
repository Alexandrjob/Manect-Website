using Manect.Interfaces;
using System;
using System.Threading.Tasks;

namespace Manect.Data
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        public Task<T> FindByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<T> ToListAsync()
        {
            throw new NotImplementedException();
        }
    }
}
