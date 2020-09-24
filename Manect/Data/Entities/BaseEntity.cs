using System;

namespace Manect.Data.Entities
{
    /// <summary>
    /// Класс в котором хранятся стандартные для всех классов свойства.
    /// </summary>
    public class BaseEntity
    {
        public int Id { get; set; }
        /// <summary>
        /// Название проекта или этапа(в зависимости от класса реализующий данный класс).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Исполнитель проекта или этапа(в зависимости от класса реализующий данный класс).
        /// </summary>  
        public ExecutorUser Executor { get; set; }

        /// <summary>
        /// Дата создания проекта или этапа(в зависимости от класса реализующий данный класс).
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Дата завершения проекта или этапа(в зависимости от класса реализующий данный класс).
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Возвращает true, если проект или этап выполнен, иначе false.
        /// </summary>
        public Status Status { get; set; }
    }
}
