using Manect.Data.Entities;

namespace Manect.Controllers.Models
{
    public class HistoryItem
    {
        public string ExecutorFirstName { get; set; }
        public string ExecutorLastName { get; set; }
        public StatusRus StatusRus  { get; set; }
        public string StageName { get; set; }
        public int StageId { get; set; }
        public string ProjectName { get; set; }
        public int ProjectId { get; set; }
    }
}
