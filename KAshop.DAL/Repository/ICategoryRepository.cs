using KAshop.DAL.DTO.Request;
using KAshop.DAL.DTO.Response;
using KAshop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAshop.DAL.Repository
{
   public interface ICategoryRepository
    {

        public Category Create(Category request);
        public List<Category> GetAll();

         Task DeleteAsync(Category category);
        Task<Category?> FindByIdAsync(int id);

    }
}
