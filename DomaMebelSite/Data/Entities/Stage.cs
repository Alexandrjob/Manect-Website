using System;

namespace DomaMebelSite.Entities
{
    /// <summary>
    /// Этап.
    /// </summary>
    public class Stage : BaseEntity
    {
        /// <summary>
        /// Дополнительные исполнители, которые могут работать над этапом.
        /// </summary>
        public string? AdditionalPerformer { get; private set; }

        /// <summary>
        /// Комментарий.
        /// </summary>
        public string? Comment { get; private set; }

        public Stage()
        {

        }

        /// <summary>
        /// Создание этапа.
        /// </summary>
        /// <param name="name"> Имя этапа. </param>
        /// <param name="expirationDate"> Окончание этапа. </param>
        /// <param name="performer"> Добавить исполнителя. </param>
        /// <param name="comment"> Комментарий этапа. </param>
        /// <param name="isDone"> Завершен этап или нет. </param>
        public Stage(string name, DateTime expirationDate, string performer = "", string comment = "", bool isDone = false)
        {
            Name = "Этап: " + name;
            ExpirationDate = expirationDate;
            AdditionalPerformer = performer;
            Comment = comment;
            IsDone = isDone;

            CreationDate = DateTime.Now;
        }
    }
}