using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace ManectTelegramBot
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTelegramBotClient(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            var client = new TelegramBotClient(configuration["Token"]);
            var webHook = $"{configuration["Url"]}/api/telegram";
            client.SetWebhookAsync(webHook).Wait();

            return serviceCollection
                .AddTransient<ITelegramBotClient>(x => client);
        }
    }
}