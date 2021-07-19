using covid192020.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace covid192020.Areas.Admin.Models
{
    public class ApplicationContext : IdentityDbContext<CustomIdentityUser>
    {
        public DbSet<CustomIdentityUser> CustomIdentityUsers { get; set; }
        public DbSet<SelectItem> SelectItems { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Treatment> Treatments{ get; set; }
        public DbSet<Dinamic> Dinamics { get; set; }
        public DbSet<Preparat> Preparats { get; set; }
        public DbSet<Kt> Kts { get; set; }
        public DbSet<IdentVirus> IdentViruses { get; set; }
        //public DbSet<TreatmentHistory> TreatmentHistories { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }
        
    }
    //Для работы с авторизацией 
    //Microsoft.AspNetCore.Identity.EntityFrameworkCore

    //Для работы с sql сервером, точнее для установки связи с ним
    //Microsoft.EntityFrameworkCore.SqlServer

    //Операции миграции ApplicationContext в базу и таблицы sql
    //Microsoft.EntityFrameworkCore.Tools

    //Связ с браузером (Для разработки)
    //Microsoft.VisualStudio.Web.BrowserLink

    //Внедрение зависимо только в метод
    //public async Task<IActionResult> ChangePassword([FromServices] ApplicationContext applicationContext)

    //...AddDefaultTokenProviders() без него не работает сброс пароля
    // точнее не работает вот эта функция ...await userManager.ResetPasswordAsync(user, token, рassword);

    // Создает файл ключа для шифрования паролей (обязательная штука если размищать сайт на REG.RU иначе вылитает авторизация)
    //services.AddDataProtection()
    //            .SetApplicationName("covid192020")
    //            .SetDefaultKeyLifetime(TimeSpan.FromDays(365))
    //            .PersistKeysToFileSystem(new DirectoryInfo(@".\\Keys\\"));
    //           // .ProtectKeysWithCertificate("thumbprint");

    // выкинить (другого) пользователя из системы
    // var u = await userManager.FindByIdAsync("ad56f3e2-0898-4321-b740-05e156f13c84");
    // var result = await userManager.UpdateSecurityStampAsync(u);
    //services.Configure<SecurityStampValidatorOptions>(options =>
    //{
    // позволяет немедленно выйти из системы после обновления статистики пользователя.
    //options.ValidationInterval = TimeSpan.Zero;
    //});
    // хотя эта функиция стала работать значительно умнее, выкидывает даже при смене роли ползователя
    // если он находится в запрещеном разделе однако если он там где разрешен его не выкадывает

    //TempData["Value"] передает значение даже при Redirect на другой контроллер/метод ViewBag так не может

    // Установка культуры в startup.cs
    // если не установить эту культуру не работае номально типа decimal в привязке и конвертации
    //var supportedCultures = new[]
    //{
    //   new CultureInfo("en-US"),
    //   new CultureInfo("en"),
    //       возможно без этих строк
    //       new CultureInfo("ru-RU"),
    //       new CultureInfo("ru")
    //};
    //app.UseRequestLocalization(new RequestLocalizationOptions
    //{
    //    DefaultRequestCulture = new RequestCulture("en-US"),
    //    SupportedCultures = supportedCultures,
    //    SupportedUICultures = supportedCultures
    //});

    //**Создание админа и ролей при первом запуске**
    //Непосредственно вызов
    //public void Configure(IApplicationBuilder app, IServiceProvider services)
    //{
    //    ...
    //    CreateAdminAndRole(services).Wait();
    //}

    //public async Task CreateAdminAndRole(IServiceProvider serviceProvider)
    //{
    //    var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    //    var UserManager = serviceProvider.GetRequiredService<UserManager<CustomIdentityUser>>();

    //    if (!await RoleManager.RoleExistsAsync("Admin"))
    //        await RoleManager.CreateAsync(new IdentityRole("Admin"));

    //    if (!await RoleManager.RoleExistsAsync("Assistant"))
    //        await RoleManager.CreateAsync(new IdentityRole("Assistant"));

    //    if (!await RoleManager.RoleExistsAsync("Medic"))
    //        await RoleManager.CreateAsync(new IdentityRole("Medic"));

    //    var user = await UserManager.FindByNameAsync("Administrator");
    //    if (user == null)
    //    {
    //        user = new CustomIdentityUser { UserFio = "Администратор", UserName = "Administrator", Email = "hhaid@outlook.com", EmailConfirmed = true };
    //        await UserManager.CreateAsync(user, "covid2174777");
    //    }
    //    if (!await UserManager.IsInRoleAsync(user, "Admin"))
    //        await UserManager.AddToRoleAsync(user, "Admin");

    //}
}
