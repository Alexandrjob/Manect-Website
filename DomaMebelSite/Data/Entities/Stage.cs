using System;

namespace DomaMebelSite.Entities
{
    /// <summary>
    /// Этап.
    /// </summary>
    public class Stage: BaseEntity
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
        public Stage(string name, DateTime expirationDate, string performer ="", string comment ="")
        {
            Name = "Этап: " + name;
            CreationDate = DateTime.Now;
            ExpirationDate = expirationDate;
            AdditionalPerformer = performer;
            Comment = comment;
        }
    }
}