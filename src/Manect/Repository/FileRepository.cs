using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Manect.Controllers.Models;
using Manect.Data;
using Manect.Data.Entities;
using Manect.Interfaces.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Manect.Repository
{
    public class FileRepository : IFileRepository
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger _logger;

        private ProjectDbContext DataContext
        {
            get
            {
                var scope = _serviceScopeFactory.CreateScope();
                return scope.ServiceProvider.GetService<ProjectDbContext>();
            }
        }

        public FileRepository(IServiceScopeFactory serviceScopeFactory, ILogger<FileRepository> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task AddFileAsync(DataToChange dataToChange)
        {
            var dataContext = DataContext;
            foreach (var file in dataToChange.Files)
            {
                //TODO: Переместить валидацию файлов в контроллер.
                if (IsNotExtensionValid(file)) break;

                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);

                if (memoryStream.Length > 5242880) break;

                var appFile = new AppFile()
                {
                    StageId = dataToChange.StageId,
                    Name = file.FileName,
                    Type = file.ContentType,
                    Content = memoryStream.ToArray()
                };
                await dataContext.AddAsync(appFile);
            }

            await dataContext.SaveChangesAsync();
            //В связи с изменением логики добавления этапа конечное сообщение на странице история будет просто говорить что в этап добавлен файл, без конкретики.
            _logger.LogInformation(
                "Время: {TimeAction}. Пользователь(Id:{ExecutorId}), {Status} файл(Id:{FileId}) в этапе(Id:{StageId}) в Проекте(Id:{ProjectId})",
                DateTime.Now, dataToChange.CurrentUserId, Status.Created, -1, dataToChange.StageId,
                dataToChange.ProjectId);
        }

        private bool IsNotExtensionValid(IFormFile file)
        {
            var fileType = file.FileName.Split(".").Last();
            foreach (var extensions in Enum.GetValues(typeof(Extensions)))
            {
                if (fileType == extensions.ToString())
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<AppFile> GetFileAsync(DataToChange dataToChange)
        {
            _logger.LogInformation(
                "Время: {TimeAction}. Пользователь(Id:{ExecutorId}) , {Status} файл(Id:{FileId}) в Проекте (Id:{ProjectId})",
                DateTime.Now, dataToChange.CurrentUserId, Status.Received, dataToChange.FileId, dataToChange.ProjectId);

            return await DataContext.Files.AsNoTracking().FirstOrDefaultAsync(f => f.Id == dataToChange.FileId);
        }

        public async Task<List<AppFile>> FileListAsync(DataToChange dataToChange)
        {
            var files = await DataContext.Files
                .Where(f => f.StageId == dataToChange.StageId)
                .Select(obj => new
                {
                    obj.Id,
                    obj.Name,
                    obj.StageId,
                    obj.Content.Length
                })
                .AsQueryable()
                .Select(f => new AppFile
                {
                    Id = f.Id,
                    Name = f.Name,
                    StageId = f.StageId,
                    Length = f.Length
                })
                .ToListAsync();

            return files;
        }

        public async Task DeleteFileAsync(DataToChange dataToChange)
        {
            var file = new AppFile
            {
                Id = dataToChange.FileId
            };
            _logger.LogInformation(
                "Время: {TimeAction}. Пользователь(Id:{ExecutorId}) , {Status} файл(Id:{FileId}) в этапе (Id:{ProjectId})",
                DateTime.Now, dataToChange.CurrentUserId, Status.Deleted, dataToChange.FileId, dataToChange.StageId);

            var dataContext = DataContext;
            dataContext.Files.Attach(file);
            dataContext.Entry(file).State = EntityState.Deleted;
            await dataContext.SaveChangesAsync();
        }
    }
}