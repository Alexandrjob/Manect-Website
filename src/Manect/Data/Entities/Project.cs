using System;
using System.Collections.Generic;

namespace Manect.Data.Entities
{
    public class Project : BaseEntity
    {
        /// <summary>
        /// Цена проекта.
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// Список этапов.
        /// </summary>
        public ICollection<Stage> Stages { get; set; }

        public Project()
        {
            Stages = new List<Stage>();
        }

        /// <summary>
        /// Создание проекта.
        /// </summary>
        /// <param name="name"> Название проекта. </param>
        /// <param name="price"> Цена проекта. </param>
        /// <param name="executor"> Исполнитель проекта. </param>
        /// <param name="stages"> Этапы проекта. </param>
        public Project(string name, int price, Executor executor, List<Stage> stages)
        {
            Name = name;
            Price = price;
            Executor = executor;
            Stages = stages;

            CreationDate = DateTime.Now;
            ExpirationDate = CreationDate.AddDays(30);
            Status = Status.Created;
        }
    }
}