using Manect.Data.Entities;
using System;

namespace Manect.DataBaseLogger
{
    public class LogItem
    {
        public int Id { get; set; }
        public int ExecutorId { get; set; }
        public int ProjectId { get; set; }
        public int StageId { get; set; }
        public int FileId { get; set; }
        public DateTime TimeAction { get; set; }
        public Status Status { get; set; }
    }
}
