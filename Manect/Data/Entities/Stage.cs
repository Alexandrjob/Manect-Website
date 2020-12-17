using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manect.Data.Entities
{
    /// <summary>
    /// Этап проекта.
    /// </summary>
    public class Stage: BaseEntity
    {
        /// <summary>
        /// Комментарий.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Список изображений этапа.
        /// </summary>
        public ICollection<AppFile> Files { get; set; }

        /// <summary>
        /// Id исполнителя.
        /// </summary>
        [ForeignKey("Executor")]
        public int ExecutorId { get; set; }

        /// <summary>
        /// Id проекта в котором был создан этап.
        /// </summary>
        // Внешний ключ
        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        public Stage() { }

        /// <summary>
        /// Создание этапа.
        /// </summary>
        /// <param name="name"> Имя этапа. </param>
        /// <param name="executor"> Исполнитель этапа. </param>
        /// <param name="comment"> Комментарий этапа. </param>
        public Stage(string name, Executor executor, string comment = "")
        {
            Name = name;
            Executor = executor;
            Comment = comment;

            CreationDate = DateTime.Now;
            ExpirationDate = CreationDate.AddDays(2);
            Status = Status.Created;
        }
    }
}