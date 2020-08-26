using System;
using System.Collections.Generic;

namespace DomaMebelSite.Entities
{
    /// <summary>
    /// Мебельный проект.
    /// </summary>
    public class Project : BaseEntity
    {
        /// <summary>
        /// Цена проекта.
        /// </summary>
        public int Price { get; private set; }

        /// <summary>
        /// Список этапов.
        /// </summary>
        public ICollection<Stage> Stages { get; private set; }

        public Project()
        {

        }
        /// <summary>
        /// Создание проекта.
        /// </summary>
        /// <param name="name"> Название проекта. </param>
        /// <param name="expirationDate"> Дата завершения проекта. </param>
        /// <param name="price"> Цена проекта. </param>
        /// <param name="stages"> Этапы проекта. </param>
        /// <param name="isDone"> Завершен проект или нет. </param>
        public Project(string name, DateTime expirationDate, int price, List<Stage> stages, bool isDone = false)
        {
            Name = name;
            ExpirationDate = expirationDate;
            Price = price;
            Stages = stages;
            IsDone = isDone;

            CreationDate = DateTime.Now;
        }
    }
}