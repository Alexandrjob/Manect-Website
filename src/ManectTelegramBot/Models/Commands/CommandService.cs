using System.Collections.Generic;
using ManectTelegramBot.Abstractions;

namespace ManectTelegramBot.Models.Commands
{
    public class CommandService : ICommandService
    {
        private readonly List<TelegramCommand> _commands;

        public CommandService()
        {
            _commands = new List<TelegramCommand>
            {
                new StartCommand()
            };
        }

        public List<TelegramCommand> GetCommands() => _commands;
    }
}