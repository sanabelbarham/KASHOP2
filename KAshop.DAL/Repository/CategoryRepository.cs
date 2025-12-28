using KAshop.DAL.Data;
using KAshop.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAshop.DAL.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;


        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
      
        public Category Create(Category request)
        {
            _context.Categories.Add(request);
            _context.SaveChanges();
            return request;
        }

        public List<Category> GetAll()
        {
    return _context.Categories.Include(c => c.Translations).ToList();
        }

        public async Task<Category?> FindByIdAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.Translations)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task  DeleteAsync(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }



    }
}
