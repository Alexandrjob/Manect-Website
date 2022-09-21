using System;
using Manect.Data.Entities;

namespace Manect.Controllers.Models
{
    public class HistoryItem
    {
        public string ExecutorFirstName { get; set; }
        public string ExecutorLastName { get; set; }
        public int ExecutorId { get; set; }
        public string ProjectName { get; set; }
        public int ProjectId { get; set; }
        public string StageName { get; set; }
        public int StageId { get; set; }
        public int FileId { get; set; }
        public string AbbreviationName { get; set; }
        public string Message { get; set; }
        public DateTime TimeAction { get; set; }
        public Status Status { get; set; }
    }
}