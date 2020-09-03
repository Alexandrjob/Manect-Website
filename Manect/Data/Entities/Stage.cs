using System;

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

        public Stage() { }

        /// <summary>
        /// Создание этапа.
        /// </summary>
        /// <param name="name"> Имя этапа. </param>
        /// <param name="expirationDate"> Окончание этапа. </param>
        /// <param name="performer"> Добавить исполнителя. </param>
        /// <param name="comment"> Комментарий этапа. </param>
        /// <param name="isDone"> Завершен этап или нет. </param>
        public Stage(string name, DateTime expirationDate, ExecutorUser executor = null, string comment = "", bool isDone = false)
        {
            Name = "Этап: " + name;
            ExpirationDate = expirationDate;
            Executor = executor;
            Comment = comment;
            IsDone = isDone;

            CreationDate = DateTime.Now;
        }
    }
}