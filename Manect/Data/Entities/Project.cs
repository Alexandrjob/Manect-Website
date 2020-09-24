using System;
using System.Collections.Generic;

namespace Manect.Data.Entities
{
    /// <summary>
    /// Мебельный проект.
    /// </summary>
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
        /// <param name="expirationDate"> Дата завершения проекта. </param>
        /// <param name="price"> Цена проекта. </param>
        /// <param name="stages"> Этапы проекта. </param>
        /// <param name="isDone"> Завершен проект или нет. </param>
        public Project(string name, int price, ExecutorUser executor, List<Stage> stages)
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