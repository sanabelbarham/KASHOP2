using KAshop.DAL.Models;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAshop.DAL.Data
{

   public class ApplicationDbContext : DbContext
    {
        ApplicationDbContext _context;
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTranslation> categoryTranslations { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {

        }
    }
}
