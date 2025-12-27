using KAshop.BLL.Service;
using KAshop.DAL.Repository;
using KAshop.DAL.Utils;

namespace KAshop.PL
{
    public class AppConfigurations
    {

        public void Config()
        {
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

            builder.Services.AddScoped<BLL.Service.ICategoryService, CategoryService>();
            builder.Services.AddScoped<ISeedData, RoleSeedData>();
            builder.Services.AddScoped<ISeedData, UserDataSeed>();

            builder.Services.AddScoped<BLL.Service.IAuthenticationService, BLL.Service.AuthenticationService>();
            builder.Services.AddScoped<IEmailSender, EmailSender>();

        }
    }
}
