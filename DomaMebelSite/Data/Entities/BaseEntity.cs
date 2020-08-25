using System;

namespace DomaMebelSite.Entities
{
    /// <summary>
    /// Класс в котором хранятся стандартные для всех классов свойства.
    /// </summary>
    public class BaseEntity
    {
        public int Id { get; protected set; }
        /// <summary>
        /// Название проекта или этапа(в зависимости от класса реализующий данный класс).
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Дата создания проекта или этапа(в зависимости от класса реализующий данный класс).
        /// </summary>
        public DateTime CreationDate { get; protected set; }

        /// <summary>
        /// Дата завершения проекта или этапа(в зависимости от класса реализующий данный класс).
        /// </summary>
        public DateTime ExpirationDate { get; protected set; }
        public bool IsDone { get; set; }
    }
}
