using Manect.Controllers.Models;
using Manect.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manect.Interfaces.IRepositories
{
    public interface IFileRepository
    {
        Task AddFileAsync(DataToChange dataToChange);
        Task DeleteFileAsync(DataToChange dataToChange);
        Task<AppFile> GetFileAsync(DataToChange dataToChange);
        Task<List<AppFile>> FileListAsync(DataToChange dataToChange);
    }
}
