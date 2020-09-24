using Manect.Data.Entities;
using System;

namespace Manect.DataBaseLogger
{
    public class LogItem
    {
        public int Id { get; set; }
        public string ExecutorName { get; set; }
        public string ProjectName { get; set; }
        public string StageName { get; set; }
        public DateTime TimeAction { get; set; }
        public Status Status { get; set; }
    }
}
