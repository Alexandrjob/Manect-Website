using System.Collections.Generic;

namespace Manect.Data.Entities
{
    public class Executor
    {
        /// <summary>
        /// Id исполнителя.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Имя исполнителя на английском(для идентификации).
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Имя исполнителя.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия исполнителя.
        /// </summary>
        public string LastName { get; set; }

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
    }
}
