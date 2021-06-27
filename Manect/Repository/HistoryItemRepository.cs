using Manect.Controllers.Models;
using Manect.Data;
using Manect.Interfaces;
using Manect.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manect.Repository
{
    public class HistoryItemRepository: IHistoryItemRepository
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IStringFormatService _messageFormatService;

        private ProjectDbContext DataContext
        {
            get
            {
                var scope = _serviceScopeFactory.CreateScope();
                return scope.ServiceProvider.GetService<ProjectDbContext>();
            }
        }

        public HistoryItemRepository(IServiceScopeFactory serviceScopeFactory, IStringFormatService messageFormatService)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _messageFormatService = messageFormatService;
        }

        public async Task<List<HistoryItem>> GetHistoryAsync()
        {
            var dataContext = DataContext;

            // Where(l => l.ExecutorId != 6) Это мой Id.
            var history = await dataContext.LogUserActivity
                .AsNoTracking()
                .Where(l => l.ExecutorId != 6)
                .OrderByDescending(l => l.TimeAction)
                .Take(10)
                .Select(s => new
                {
                    s.ExecutorId,
                    s.ProjectId,
                    s.StageId,
                    s.FileId,
                    s.Status,
                    s.TimeAction
                })
                .AsQueryable()
                .Select(h => new HistoryItem
                {
                    ExecutorId = h.ExecutorId,
                    ProjectId = h.ProjectId,
                    StageId = h.StageId,
                    FileId = h.FileId,
                    Status = h.Status,
                    TimeAction = h.TimeAction
                })
                .ToListAsync();

            var AllIds = history.Select(l => new { l.ExecutorId, l.ProjectId, l.StageId }).ToArray();

            //TODO: Оптимизировать.
            var executors = await dataContext.Executors.AsNoTracking().Where(e => AllIds.Select(a => a.ExecutorId).Contains(e.Id)).ToListAsync();
            var projects = await dataContext.Projects.AsNoTracking().Where(e => AllIds.Select(a => a.ProjectId).Contains(e.Id)).ToListAsync();
            var stages = await dataContext.Stages.AsNoTracking().Where(e => AllIds.Select(a => a.StageId).Contains(e.Id)).ToListAsync();

            foreach (var h in history)
            {
                foreach (var e in executors)
                {
                    if (h.ExecutorId == e.Id)
                    {
                        h.ExecutorFirstName = e.FirstName;
                        h.ExecutorLastName = e.LastName;
                    }
                }

                foreach (var p in projects)
                {
                    if (h.ProjectId == p.Id)
                    {
                        h.ProjectName = p.Name;
                    }
                }

                foreach (var s in stages)
                {
                    if (h.StageId == s.Id)
                    {
                        h.StageName = s.Name;
                    }
                }
            }

            //var messages = new List<string>();

            foreach (var item in history)
            {
                foreach (var messageFormat in _messageFormatService.GetStringFormats())
                {
                    if (messageFormat.Contains(item))
                    {
                        item.Message = messageFormat.Execute(item);
                        break;
                    }
                }
            }

            foreach (var h in history)
            {
                h.AbbreviationName = "" + h.ExecutorFirstName.First() + h.ExecutorLastName.First();
            }

            return history;
        }
    }
}
