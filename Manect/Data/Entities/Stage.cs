using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manect.Data.Entities
{
    /// <summary>
    /// Этап.
    /// </summary>
    public class Stage : BaseEntity
    {
        /// <summary>
        /// Комментарий.
        /// </summary>
        public string Comment { get; set; }

        // Внешний ключ
        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        public Stage() { }

        /// <summary>
        /// Создание этапа.
        /// </summary>
        /// <param name="name"> Имя этапа. </param>
        /// <param name="executor"> Добавить исполнителя. </param>
        /// <param name="comment"> Комментарий этапа. </param>
        public Stage(string name, ExecutorUser executor, string comment = "")
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