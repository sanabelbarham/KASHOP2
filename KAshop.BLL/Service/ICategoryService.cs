
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

        Task<CategoryResponse> CreateCategory(CategoryRequest request);
        Task<List<CategoryResponse>> GetCategory();
        Task<BaseResponce> DeleteCategoryAsync(int id);
        Task<BaseResponce> UpdateCategoryAsync(int id, CategoryRequest request);


    }
}
