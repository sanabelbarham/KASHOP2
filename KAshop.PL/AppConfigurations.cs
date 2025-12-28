    using KAshop.BLL.Service;
    using KAshop.DAL.Repository;
    using KAshop.DAL.Utils;
    using Microsoft.AspNetCore.Identity.UI.Services;

    namespace KAshop.PL
    {
        public static class AppConfigurations
        {

            public static void Config(IServiceCollection Services)
            {

              Services.AddScoped<ICategoryRepository, CategoryRepository>();

               Services.AddScoped<BLL.Service.ICategoryService, CategoryService>();
                Services.AddScoped<ISeedData, RoleSeedData>();
                Services.AddScoped<ISeedData, UserDataSeed>();

                Services.AddScoped<BLL.Service.IAuthenticationService, BLL.Service.AuthenticationService>();
                Services.AddScoped<IEmailSender, EmailSender>();

            }
        }
    }
