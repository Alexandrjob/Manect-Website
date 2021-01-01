using System.Collections.Generic;

namespace ManectTelegramBot.Abstractions
{
    public interface ICommandService
    {
        List<TelegramCommand> GetCommands();
    }
}