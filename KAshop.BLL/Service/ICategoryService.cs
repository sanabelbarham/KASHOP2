
using KAshop.DAL.DTO.Request;
using KAshop.DAL.DTO.Response;
using KAshop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAshop.BLL.Service
{
    public interface ICategoryService
    {

        public CategoryResponse CreateCategory(CategoryRequest request);
        public List<CategoryResponse> GetCategory();
        Task<BaseResponce> DeleteCategoryAsync(int id);


    }
}
