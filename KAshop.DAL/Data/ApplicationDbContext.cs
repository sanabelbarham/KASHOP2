using KAshop.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KAshop.DAL.Data
{

   public class ApplicationDbContext : IdentityDbContext <ApplicationUser> //this will creat the DbSet for all the models built in inside the ApplicationUser
                                                                           //
                                                                           //that inhert from IdentityUser and we must specify this <ApplicationUser> since the default is dealing with IdentityUser
    {
        //We did this  insted of inheriting from the DbContext


        ApplicationDbContext _context;
        private readonly HttpContextAccessor _httpContextAccessor;

        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTranslation> categoryTranslations { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,HttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        //changing the dafult names for the 7 tables in Identity to names from my own choice
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim <string>>().ToTable("UserClaims");
            builder.Entity<IdentityRoleClaim <string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserLogin <string>>().ToTable("UserLogins");
            builder.Entity<IdentityUserToken <string>>().ToTable("UserTokens");
          
        }
        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries<BaseModel>();
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            foreach (var entiryEntry in entries)
            {
             if(entiryEntry.State == EntityState.Added)
                {
                    entiryEntry.Property(x => x.CreatedBy).CurrentValue = currentUserId;
                    entiryEntry.Property(x => x.CreatedAt).CurrentValue = DateTime.UtcNow;
                } else if(entiryEntry.State == EntityState.Modified)
                {
                    entiryEntry.Property(x => x.UpdatedBy).CurrentValue = currentUserId;
                    entiryEntry.Property(x => x.UpdatedAt).CurrentValue = DateTime.UtcNow;
                }
            }

            return base.SaveChanges();
        }
    }
}
