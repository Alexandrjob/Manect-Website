using Manect.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manect.Data
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        public Task<T> FindByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }
    }
}
