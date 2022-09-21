# Manect
Task-сайт и телеграм-бот, созданные, для контроля над проектами которые ведут сотрудники в кампании.
# Demo


# Usage
Создать две базы данных.    
В файле appsetings.json поменять строку подкючения к вашим базам данных.    
Для создания таблиц в базе данных, в окне терминала ide, запустим команды.

`dotnet ef migrations add createMigration --project src/Manect --context IdentityDbContext --output-dir Migrations/IdentityDbContext`  
`dotnet ef migrations add createMigration --project src/Manect --context ProjectDbContext --output-dir Migrations/ProjectDbContext`  
`dotnet ef database update --project src/Manect --context IdentityDbContext`     
`dotnet ef database update --project src/Manect --context ProjectDbContext`

### Закоментировать код в TelegramController 

```
public TelegramController()
        // UserManager<ApplicationUser> userManager,
        // SignInManager<ApplicationUser> signInManager,
        // ICommandService commandService,
        // ITelegramBotClient telegramBotClient,
        // ITelegramRepository telegramRepository,
        // IExecutorRepository executorRepository)
    {
        // _userManager = userManager;
        // _signInManager = signInManage
        // _commandService = commandService;
        // _telegramBotClient = telegramBotClient;
        //
        // _telegramRepository = telegramRepository;
        // _executorRepository = executorRepository;

        DataToChange = new DataToChange();
    }

```

### Закоментировать код в Sturtup

```
//Add Telegram bot
services.AddScoped<ICommandService, CommandService>();
services.AddScoped<IStringFormatService, StringFormatService>();
//services.AddScoped<ServiceTelegramMessage>();
//services.AddTelegramBotClient(Configuration);
```

