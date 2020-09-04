using Manect.Identity;
using System;
using System.Collections.Generic;

namespace Manect.Data.Entities
{
    public class ExecutorUser
    {
        /// <summary>
        /// Id исполнителя.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Имя исполнителя.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Email исполнителя.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Список проектов исполнителя.
        /// </summary>
        public ICollection<Project> Projects { get; set; }

        /// <summary>
        /// Список этапов исполнителя.
        /// </summary>
        public ICollection<Stage> Stages { get; set; }

        public ExecutorUser()
        {

        }

        public static implicit operator ExecutorUser(ApplicationUser v)
        {
            throw new NotImplementedException();
        }
    }
}
