using Manect.Controllers.Models;
using Manect.Data;
using Manect.Data.Entities;
using Manect.Interfaces.IRepositories;
using ManectTelegramBot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

namespace Manect.Repository
{
    public class TelegramRepository : ITelegramRepository
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private ProjectDbContext DataContext
        {
            get
            {
                var scope = _serviceScopeFactory.CreateScope();
                return scope.ServiceProvider.GetService<ProjectDbContext>();
            }
        }

        public TelegramRepository(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<long> GetTelegramIdAsync(DataToChange dataToChange)
        {
            var telegramId = await DataContext.Executors.Where(executor => executor.Id == dataToChange.ExecutorId)
                                                        .Select(executor => executor.TelegramId)
                                                        .FirstOrDefaultAsync();
            return telegramId;
        }

        public async Task AddTelegramIdAsync(DataToChange dataToChange)
        {
            var dataContext = DataContext;
            var currentExecutor = await dataContext.Executors.FirstOrDefaultAsync(executor => executor.Id == dataToChange.CurrentUserId);

            currentExecutor.TelegramId = dataToChange.TelegramId;
            dataContext.Entry(currentExecutor).State = EntityState.Modified;
            await dataContext.SaveChangesAsync();
        }

        public async Task<MessageObject> GetInformationForMessageAsync(DataToChange dataToChange)
        {
            var dataContext = DataContext;

            #region projectInfo
            var projectInfo = await dataContext.Projects
               .AsNoTracking()
               .Where(project => project.Id == dataToChange.ProjectId)
               .Select(pr => new
               {

                   pr.Name,
                   pr.Executor.FirstName,
                   pr.Executor.LastName,
                   StageName = pr.Stages.Where(stage => stage.Id == dataToChange.StageId).Select(s => s.Name).FirstOrDefault()

               })
               .AsQueryable()
               .Select(project => new MessageObject
               {
                   StageName = project.StageName,
                   ProjectName = project.Name,

                   ExecutorProject = new ExecutorProject()
                   {
                       FirstName = project.FirstName,
                       LastName = project.LastName
                   }
               })
               .FirstOrDefaultAsync();
            #endregion

            #region senderInfo
            var messageObject = await dataContext.Executors
                .AsNoTracking()
                .Where(exe => exe.Id == dataToChange.CurrentUserId)
                .Select(exe => new
                {
                    exe.FirstName,
                    exe.LastName
                })
                .AsQueryable()
                .Select(messageObject => new MessageObject
                {
                    SenderExecutor = new SenderExecutor()
                    {
                        FirstName = messageObject.FirstName,
                        LastName = messageObject.LastName
                    }
                })
                .FirstOrDefaultAsync();
            #endregion]

            #region recipientInfo
            var recipientExecutor = await dataContext.Executors
                .AsNoTracking()
                .Where(exe => exe.Id == dataToChange.ExecutorId)
                .Select(exe => new
                {
                    exe.FirstName,
                    exe.LastName,
                    exe.TelegramId
                })
                .AsQueryable()
                .Select(recipientExecutor => new RecipientExecutor
                {
                    FirstName = recipientExecutor.FirstName,
                    LastName = recipientExecutor.LastName,
                    TelegramId = recipientExecutor.TelegramId
                })
                .FirstOrDefaultAsync();
            #endregion

            messageObject.RecipientExecutor = recipientExecutor;
            messageObject.ExecutorProject = projectInfo.ExecutorProject;
            messageObject.ProjectName = projectInfo.ProjectName;
            messageObject.StageName = projectInfo.StageName;

            return messageObject;
        }

        public async Task<Executor> GetFullNameAsync(DataToChange dataToChange)
        {
            var executor = await DataContext.Executors
               .AsNoTracking()
               .Where(exe => exe.Id == dataToChange.CurrentUserId)
               .Select(exe => new
               {
                   exe.FirstName,
                   exe.LastName
               })
               .AsQueryable()
               .Select(executor => new Executor
               {
                   FirstName = executor.FirstName,
                   LastName = executor.LastName
               })
               .FirstOrDefaultAsync();

            return executor;
        }
    }
}
