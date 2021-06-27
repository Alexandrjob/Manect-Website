using Manect.Controllers.Models;
using Manect.Data.Entities;
using Manect.Interfaces;
using System;

namespace Manect.Services.StringFormats
{
    public class FileStringFormat: StringFormat
    {
        public override bool Contains(HistoryItem item)
        {
            //Если id file не равняется дефолт, значит изменения произошли в файле.
            if (item.FileId == -1)
            {
                return true;
            }

            return false;
        }

        public override string Execute(HistoryItem item)
        {
            //Лиза выполнила 5 этап(Создание конечного макета) в проекте Шкаф 6.
            var statusRus = Convert.ToString((StatusRus)item.Status).ToLower();
            var message = string.Format("{0} {1} {2}(а) файл в этапe: {3} в проекте: {4}.", item.ExecutorFirstName, item.ExecutorLastName, statusRus, item.StageName, item.ProjectName);
            return message;
        }
    }
}
