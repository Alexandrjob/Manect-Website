using System;

namespace Manect.DataBaseLogger
{
    public class HistoryItem
    {
        public int Id { get; set; }
        public string ExecutorName { get; set; }
        public string ProjectName { get; set; }
        public string StageName { get; set; }
        public DateTime TimeAction { get; set; }
        //TODO: Считаю нужно добавить поле которое будет указывать на тип дйствия пользователя с обьектом при помощи перечисления
    }
}
