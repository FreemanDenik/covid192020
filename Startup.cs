using covid192020.Areas.Admin.Models;
using covid192020.Models.Entities;
using covid192020.Models.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace covid192020
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }
        public string ConnectionStringName { get; set; }
        public string AddDataProtectionFolder { get; set; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
            ConnectionStringName = env.IsDevelopment() ? "LocalConnection" : "WebConnection";
            AddDataProtectionFolder = env.IsDevelopment() ? "." : "..";
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Создает файл ключа для шифрования паролей
            services.AddDataProtection()
                .SetApplicationName("covid192020")
                .SetDefaultKeyLifetime(TimeSpan.FromDays(365))
                .PersistKeysToFileSystem(new DirectoryInfo(@$"{AddDataProtectionFolder}\\Keys\\"));
               // .ProtectKeysWithCertificate("thumbprint");

            services.AddTransient<IPatientRepository, PatientRepository>();

            services.AddDbContext<ApplicationContext>(options =>
                       options.UseSqlServer(Configuration.GetConnectionString(ConnectionStringName)));

            services.AddIdentity<CustomIdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();
            //AddDefaultTokenProviders() без него не работаетсброс пароля

            //services.AddDistributedMemoryCache();
            services.AddSession();



            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            //{
            // add an instance of the patched manager to the options:
            //   options.CookieManager = new ChunkingCookieManager();
            /*options.Cookie.SameSite = SameSiteMode.Strict;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;*/
            //});
           /* services.Configure<CookieOptions>(option=> {

                option.Secure = true;
                option.SameSite = SameSiteMode.Unspecified;
                option.HttpOnly = true;
            
            });*/
            services.ConfigureApplicationCookie(options =>
            {
                //options.Cookie.SameSite = SameSiteMode.None;
                //options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
              
                options.Cookie.Name = "covid192020";
                //options.Cookie.MaxAge = TimeSpan.FromSeconds(5);
                //options.Cookie.Path = "/";
                options.Cookie.IsEssential = true;
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(365);
                options.LoginPath = "/Account/Login"; 

                options.AccessDeniedPath = "/Account/AccessDenied"; 
      
                //options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });
            /*services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
                options.OnAppendCookie = cookieContext => cookieContext.CookieOptions.SameSite = SameSiteMode.Unspecified;
                options.OnDeleteCookie = cookieContext => cookieContext.CookieOptions.SameSite = SameSiteMode.Unspecified;
            });*/
            services.Configure<CookieTempDataProviderOptions>(options => {
                // судя по тому что видно это куки динамичной переменной TempData[""] = ""
                options.Cookie.Name = "TempDataCookie";
            });
            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                // позволяет немедленно выйти из системы после обновления статистики пользователя.
                options.ValidationInterval = TimeSpan.Zero;
            });
            services.Configure<IdentityOptions>(options =>
            {
            //  должен ли пароль содержать цифру true = да
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 0;
                
            //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            //    options.Lockout.MaxFailedAccessAttempts = 10;
            //    options.Lockout.AllowedForNewUsers = true;
            //    options.User.RequireUniqueEmail = true;
            });
             services.AddHsts(options =>
             {
                 options.Preload = true;
                 options.IncludeSubDomains = true;
                 options.MaxAge = TimeSpan.FromDays(365);
                 /*options.ExcludedHosts.Add("example.com");
                 options.ExcludedHosts.Add("www.example.com");*/
             });
            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status301MovedPermanently;
                options.HttpsPort = 443;
            });
            //services.AddAuthentication(CookieAuthenticationDefaults.ReturnUrlParameter).AddCookie();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            // если не установить эту культуру не работае номально типа decimal в привязке и конвертации
            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("en"),
                new CultureInfo("ru-RU"),
                new CultureInfo("ru")
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            //app.UseExceptionHandler("/Error");
            //app.UseStatusCodePages();
            app.UseStatusCodePagesWithReExecute("/Error", "?code={0}");
            
            app.UseStaticFiles();

            //app.UseCookiePolicy();
            app.UseSession();

            app.UseRouting();

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

           // app.UseCookiePolicy();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                       name: "areas",
                       pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                /*endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Patient}-{PatientId:int}/{action=Index}");*/
               endpoints.MapControllerRoute(
                    name: "default",
                    defaults: new {action = "Index"},
                    pattern: "{controller=Patient}/{Page:int}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Patient}/{action=Treatment}/{PatientId:int}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            
            CreateAdminAndRole(services).Wait();

        }
        public async Task CreateAdminAndRole(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<CustomIdentityUser>>();
   
            if (!await RoleManager.RoleExistsAsync("Admin"))
                 await RoleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await RoleManager.RoleExistsAsync("Assistant"))
                await RoleManager.CreateAsync(new IdentityRole("Assistant"));

            if (!await RoleManager.RoleExistsAsync("Medic"))
                await RoleManager.CreateAsync(new IdentityRole("Medic"));

            var user = await UserManager.FindByNameAsync("Administrator");
            if (user == null)
            {
                user = new CustomIdentityUser { UserFio="Администратор", UserName = "Administrator", Email = "hhaid@outlook.com", EmailConfirmed = true };
                await UserManager.CreateAsync(user, "covid2174777");
            }
            if (!await UserManager.IsInRoleAsync(user, "Admin"))
                 await UserManager.AddToRoleAsync(user, "Admin");

        }
    }
}
