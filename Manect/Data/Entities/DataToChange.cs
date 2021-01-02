using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manect.Data.Entities
{
    public class DataToChange
    {
        /// <summary>
        /// Id текущего пользователя.
        /// </summary>
        public int CurrentUserId { get; set; }

        /// <summary>
        /// Id пользователя.
        /// </summary>
        public int ExecutorId { get; set; }

        /// <summary>
        /// Id проекта.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Id этапа.
        /// </summary>
        public int StageId { get; set; }

        /// <summary>
        /// Id файла.
        /// </summary>
        public int FileId { get; set; }

        /// <summary>
        /// Id чата в Телеграмм.
        /// </summary>
        public long TelegramId { get; set; }

        /// <summary>
        /// Статус обьекта.
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// Список файлов.
        /// </summary>
        public IList<IFormFile> Files { get; set; }
    }
}