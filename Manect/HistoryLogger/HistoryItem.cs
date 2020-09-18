using System;

namespace Manect.HistoryLogger
{
    public class HistoryItem
    {
        //TODO: реализовать поля класса
        public string ExecutorName { get; set; }
        public string ProjectName { get; set; }
        public string StageName { get; set; }
        public DateTime TimeAction { get; set; }
    }
}
