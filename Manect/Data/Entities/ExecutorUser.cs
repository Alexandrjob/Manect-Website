using System.Collections.Generic;

namespace Manect.Data.Entities
{
    public class ExecutorUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<Stage> Stages { get; set; }
    }
}
