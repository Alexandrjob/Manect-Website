using System;
using Manect.Controllers.Models;
using Manect.Data.Entities;
using Manect.Interfaces;

namespace Manect.Services.StringFormats
{
    public class StageStringFormat : StringFormat
    {
        public override bool Contains(HistoryItem item)
        {
            //Если id file равняется дефолт, значит изменения произошли в этапе.
            if (item.StageId != default && item.FileId == default)
            {
                return true;
            }

            return false;
        }

        public override string Execute(HistoryItem item)
        {
            //Лиза выполнила 5 этап(Создание конечного макета) в проекте Шкаф 6.
            var statusRus = Convert.ToString((StatusRus) item.Status).ToLower();
            var message = string.Format("{0} {1} {2}(а) этап: {3}/n В проекте: {4}.", item.ExecutorFirstName,
                item.ExecutorLastName, statusRus, item.StageName, item.ProjectName);
            return message;
        }
    }
}