using ManectTelegramBot.Models;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace ManectTelegramBot.Service
{
    public class ServiceTelegramMessage
    {
        private readonly ITelegramBotClient _telegramBotClient;
        public ServiceTelegramMessage(ITelegramBotClient telegramBotClient)
        {
            _telegramBotClient = telegramBotClient;
        }

        private string GenerateMessage(MessageObject messageObject)
        {

            var recipientName = messageObject.RecipientExecutor.FirstName;
            var stageName = messageObject.StageName;
            var project = messageObject.ProjectName;
            var executorProjectName = messageObject.ExecutorProject.FirstName + " " + messageObject.ExecutorProject.LastName;
            var senderName = messageObject.SenderExecutor.FirstName + " " + messageObject.SenderExecutor.LastName;

            var message = string.Format("{0}, вам делегировали этап: {1}\nВ проекте: {2}.\nИсполнитель проекта: {3}.\nПередал: {4}.",
                                        recipientName, stageName, project, executorProjectName, senderName);
            return message;
        }

        public async Task Send(long chatId, MessageObject messageObject)
        {
            var message = GenerateMessage(messageObject);
            try
            {
                await _telegramBotClient.SendTextMessageAsync(chatId, message);
            }
            catch (Exception e)
            {
                var senderName = messageObject.SenderExecutor.FirstName + " " + messageObject.SenderExecutor.LastName;
                var recipientName = messageObject.RecipientExecutor.FirstName + " " + messageObject.RecipientExecutor.LastName;
                message = string.Format("{0} пытался отправить сообщение {1}.\nОшибка: " + e.Message + ".", senderName, recipientName);
                await _telegramBotClient.SendTextMessageAsync(453457635, message, parseMode: ParseMode.Markdown);
            }
        }
    }
}
