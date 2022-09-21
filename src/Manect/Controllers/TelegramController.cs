using System.Threading.Tasks;
using Manect.Controllers.Models;
using Manect.Identity;
using Manect.Interfaces.IRepositories;
using ManectTelegramBot.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Manect.Controllers
{
    [ApiController]
    [Route("api/telegram")]
    public class TelegramController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IExecutorRepository _executorRepository;
        private readonly ITelegramRepository _telegramRepository;

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ICommandService _commandService;

        public DataToChange DataToChange { get; set; }

        public TelegramController()
            // UserManager<ApplicationUser> userManager,
            //                           SignInManager<ApplicationUser> signInManager,
            // ICommandService commandService,
            // ITelegramBotClient telegramBotClient,
            // ITelegramRepository telegramRepository,
            // IExecutorRepository executorRepository)
        {
            // _userManager = userManager;
            // _signInManager = signInManager;;

            // _commandService = commandService;
            // _telegramBotClient = telegramBotClient;
            //
            // _telegramRepository = telegramRepository;
            // _executorRepository = executorRepository;

            DataToChange = new DataToChange();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Started");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            if (update == null) return Ok();

            var message = update.Message;

            if (message.Type != MessageType.Text)
            {
                var chatId = message.Chat.Id;
                await _telegramBotClient.SendTextMessageAsync(chatId,
                    "Мой создатель не давай мне инструкций как отвечать на это.", parseMode: ParseMode.Markdown);
                return Ok();
            }

            string loginCommandName = @"/login";
            var isContains = message.Text.Contains(loginCommandName);

            if (isContains)
            {
                var chatId = message.Chat.Id;
                //Предполагается, что вводимые пользователем данные будут содержать 3 слова, одно - команда, два других email и password.
                string[] textArray = message.Text.Split(' ');
                if (textArray.Length == 1)
                {
                    await _telegramBotClient.SendTextMessageAsync(chatId,
                        "Для получения уведомлений нужно пройти аутентификацию.\n" +
                        "Напиши: \"/login email password\".\nГде email твой email а password твой пароль.\n" +
                        "Важно! Соблюдай формат текста в примере - после каждого слова ПРОБЕЛ",
                        parseMode: ParseMode.Markdown);
                    return Ok();
                }

                if (textArray.Length != 3)
                {
                    await _telegramBotClient.SendTextMessageAsync(chatId,
                        "Я не понимаю формат этого сообщения, может мой создатель допустил ошибку?(Нет)\nОтправь мне /login, чтобы я тебе помог.",
                        parseMode: ParseMode.Markdown);
                    return Ok();
                }

                string email = textArray[1];
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    await _telegramBotClient.SendTextMessageAsync(chatId,
                        "Твой email в базе не найден, ты точно в ней есть?", parseMode: ParseMode.Markdown);
                    return Ok();
                }

                string password = textArray[2];
                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (result.Succeeded)
                {
                    var messageTelegramId = update.Message.Chat.Id;
                    var currentUserId = await _executorRepository.FindUserIdByEmailOrDefaultAsync(email);

                    DataToChange.CurrentUserId = currentUserId;
                    var executorName = await _telegramRepository.GetFullNameAsync(DataToChange);
                    var telegramId = await _telegramRepository.GetTelegramIdAsync(DataToChange);
                    string text;
                    if (telegramId != messageTelegramId)
                    {
                        DataToChange.TelegramId = messageTelegramId;
                        await _telegramRepository.AddTelegramIdAsync(DataToChange);
                        text = string.Format("Аутентификация прошла успешно!\n{0} {1} теперь я буду часто писать вам)",
                            executorName.FirstName, executorName.LastName);
                        await _telegramBotClient.SendTextMessageAsync(chatId, text, parseMode: ParseMode.Markdown);
                        return Ok();
                    }

                    text = string.Format("{0} {1}, я уже вас знаю) второй раз можно не здороваться.",
                        executorName.FirstName, executorName.LastName);
                    await _telegramBotClient.SendTextMessageAsync(chatId, text, parseMode: ParseMode.Markdown);
                    return Ok();
                }

                await _telegramBotClient.SendTextMessageAsync(chatId, "Хммм, пароли не совпадают, может опечатка?",
                    parseMode: ParseMode.Markdown);
                return Ok();
            }

            foreach (var command in _commandService.GetCommands())
            {
                if (command.Contains(message))
                {
                    await command.Execute(message, _telegramBotClient);
                    return Ok();
                }
            }

            await _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                "Мой создатель не давай мне инструкций как отвечать на это.");
            return Ok();
        }
    }
}