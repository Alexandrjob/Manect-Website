using System.Collections.Generic;
using System.Threading.Tasks;
using Manect.Controllers.Models;
using Manect.Data.Entities;

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