using System.Threading;
using System.Threading.Tasks;
using ManectTelegramBot.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ManectTelegramBot.Models.Commands
{
    public class StartCommand : TelegramCommand
    {
        public override string Name => @"/start";

        public override bool Contains(Message message)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Name);
        }

        public override async Task Execute(Message message, ITelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;
            await botClient.SendTextMessageAsync(chatId, "Запуск ядра прогрмаммы...", parseMode: ParseMode.Markdown);
            Thread.Sleep(3000);

            await botClient.SendTextMessageAsync(chatId, "Запуск ядра завершен успешно.",
                parseMode: ParseMode.Markdown);
            Thread.Sleep(1500);

            await botClient.SendTextMessageAsync(chatId, "Диагнотика встроеных команд...",
                parseMode: ParseMode.Markdown);
            Thread.Sleep(2500);

            await botClient.SendTextMessageAsync(chatId, "Диагнотика завершена успешна.",
                parseMode: ParseMode.Markdown);
            Thread.Sleep(1000);

            await botClient.SendTextMessageAsync(chatId, "Запуск алгоритмов общения...", parseMode: ParseMode.Markdown);
            Thread.Sleep(2000);
            await botClient.SendTextMessageAsync(chatId, "Привет!", parseMode: ParseMode.Markdown);
            Thread.Sleep(1000);
            await botClient.SendTextMessageAsync(chatId, "Чтобы получать уведомления нужно пройти аутентификацию.\n" +
                                                         "Напиши: \"/login email password\".\nГде email твой email а password твой пароль.\n" +
                                                         "Важно! Соблюдай формат текста в примере - после каждого слова ПРОБЕЛ",
                parseMode: ParseMode.Markdown);
        }
    }
}