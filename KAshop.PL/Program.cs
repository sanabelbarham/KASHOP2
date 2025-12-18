using KAshop.BLL.Service;
using KAshop.DAL;
using KAshop.DAL.Models;
using KAshop.DAL.Data;
using KAshop.DAL.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using KAshop.DAL.Utils;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
namespace KAshop.PL

{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddLocalization(options => options.ResourcesPath = "");
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
             
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            Test test = new Test();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            const string defaultCulture = "en";

            var supportedCultures = new[]
            {
            new CultureInfo(defaultCulture),
           new CultureInfo("ar")
};

            builder.Services.Configure<RequestLocalizationOptions>(options => {
                options.DefaultRequestCulture = new RequestCulture(defaultCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders.Clear();
                options.RequestCultureProviders.Add(new QueryStringRequestCultureProvider
                {
                    QueryStringKey = "Lang"
                });
            });

            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

            builder.Services.AddScoped<BLL.Service.ICategoryService, CategoryService>();
            builder.Services.AddScoped<ISeedData, RoleSeedData>();
            builder.Services.AddScoped<ISeedData, UserDataSeed>();
            builder.Services.AddScoped<BLL.Service.IAuthenticationService,BLL.Service.AuthenticationService>();

        

         var app = builder.Build();
            app.UseRequestLocalization();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.MapOpenApi();
            }

            //it will deleate the objects created once the scope is out 
            using (var scope = app.Services.CreateScope())
            {
                var Services = scope.ServiceProvider;
                var seeders=Services.GetServices<ISeedData>();
                foreach (var seeder in seeders)
                {
                    await seeder.DataSeed();
                }


            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
