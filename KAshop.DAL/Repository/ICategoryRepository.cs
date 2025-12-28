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

        public Category CreateAsync(Category request);
        Task<List<Category>> GetAllAsync();
        Task DeleteAsync(Category category);
        Task<Category?> UpdateAsync(Category category);
        Task<Category?> FindByIdAsync(int id);

    }

}
