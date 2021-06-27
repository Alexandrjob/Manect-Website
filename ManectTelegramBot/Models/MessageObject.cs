namespace ManectTelegramBot.Models
{
    public class MessageObject
    {
        public SenderExecutor SenderExecutor { get; set; }

        public string StageName { get; set; }

        public string ProjectName { get; set; }

        public ExecutorProject ExecutorProject { get; set; }

        public RecipientExecutor RecipientExecutor { get; set; }
    }


    public class SenderExecutor: FullName
    {

    }

    public class ExecutorProject: FullName
    {

    }

    public class RecipientExecutor: FullName
    {
        public long TelegramId { get; set; }
    }

    public class FullName
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
