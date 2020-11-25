using Manect.Data;
using Manect.DataBaseLogger;
using Manect.Identity;
using Manect.Interfaces;
using Manect.Services;
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
                        opts.Password.RequiredLength = 5;   // ����������� �����
                        opts.Password.RequireNonAlphanumeric = false;   // ��������� �� �� ���������-�������� �������
                        opts.Password.RequireLowercase = false; // ��������� �� ������� � ������ ��������
                        opts.Password.RequireUppercase = false; // ��������� �� ������� � ������� ��������
                        opts.Password.RequireDigit = false; // ��������� �� �����
                    })
                    .AddEntityFrameworkStores<IdentityDbContext>()
                    .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Home/Login";
            });

            services.AddScoped<IDataRepository, DataRepository>();
            services.AddScoped<ISyncTables, SyncTables>();

            services.AddAuthorization();
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IServiceScopeFactory serviceScopeFactory)
        {
            //app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseRouting();

            loggerFactory.AddDataBase("logger.txt", serviceScopeFactory);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
