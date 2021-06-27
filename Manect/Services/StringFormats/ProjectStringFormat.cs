using Manect.Controllers.Models;
using Manect.Data.Entities;
using Manect.Interfaces;
using System;

namespace Manect.Services.StringFormats
{
    public class ProjectStringFormat: StringFormat
    {
        public override bool Contains(HistoryItem item)
        {
            //Если id stage равняется дефолт, значит изменения произошли в проекте.
            if(item.StageId == default)
            {
                return true;
            }

            return false;
        }

        public override string Execute(HistoryItem item)
        {
            //Лиза выполнила  этап: Создание конечного макета в проекте: Шкаф.</div>
            //                        10 сент в 12:35

            var statusRus = Convert.ToString((StatusRus)item.Status).ToLower();
            var message = string.Format("{0} {1} {2}(а) проект: {3}.", item.ExecutorFirstName, item.ExecutorLastName, statusRus, item.ProjectName);
            return message;
        }
    }
}
