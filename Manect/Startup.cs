using Manect.Data;
using Manect.DataBaseLogger;
using Manect.Identity;
using Manect.Interfaces;
using Manect.Services;
using ManectTelegramBot;
using ManectTelegramBot.Abstractions;
using ManectTelegramBot.Models.Commands;
using ManectTelegramBot.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Manect
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ProjectDbContext>(c =>
                c.UseSqlServer(Configuration.GetConnectionString("ProjectConnection")));

            services.AddDbContext<IdentityDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection"), providerOptions => providerOptions.EnableRetryOnFailure()))
                    .AddIdentity<ApplicationUser, IdentityRole>(opts =>
                    {
                        opts.Password.RequiredLength = 5;   // минимальная длина
                        opts.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
                        opts.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
                        opts.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
                        opts.Password.RequireDigit = false; // требуются ли цифры
                    })
                    .AddEntityFrameworkStores<IdentityDbContext>()
                    .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Home/Login";
            });

            services.AddScoped<IDataRepository, DataRepository>();
            services.AddScoped<ISyncTables, SyncTables>();
            //Add Telegram bot
            services.AddScoped<ICommandService, CommandService>();
            services.AddScoped<IMessageFormatService, MessageFormatService>();
            //services.AddScoped<ServiceTelegramMessage>();
            //services.AddTelegramBotClient(Configuration);

            services.AddAuthorization();
            services.AddControllersWithViews();

            services.AddControllers().AddNewtonsoftJson();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IServiceScopeFactory serviceScopeFactory)
        {
            app.UseStaticFiles();
            app.UseRouting();

            loggerFactory.AddDataBase(serviceScopeFactory);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
