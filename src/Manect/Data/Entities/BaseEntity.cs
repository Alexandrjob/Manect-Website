using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manect.Data.Entities
{
    /// <summary>
    /// Класс в котором хранятся стандартные свойства для всех классов.
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
        public Executor Executor { get; set; }

        /// <summary>
        /// Id исполнителя.
        /// </summary>
        [ForeignKey("Executor")]
        public int ExecutorId { get; set; }

        /// <summary>
        /// Дата создания проекта или этапа(в зависимости от класса реализующий данный класс).
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Дата завершения проекта или этапа(в зависимости от класса реализующий данный класс).
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Отображает состояние обьекта.
        /// </summary>
        public Status Status { get; set; }
    }
}