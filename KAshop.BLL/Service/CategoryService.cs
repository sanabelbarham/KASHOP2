using KAshop.DAL.Repository;
using KAshop.DAL.DTO.Request;
using KAshop.DAL.DTO.Response;
using KAshop.DAL.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAshop.BLL.Service
{
     public class CategoryService:ICategoryService
    {

        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository )
        {
            _categoryRepository = categoryRepository;
        }

        public CategoryResponse CreateCategory(CategoryRequest request)
        {
            var category = request.Adapt<Category>();
            _categoryRepository.Create(category);
            var changeToCategoryResponceFormate = category.Adapt<CategoryResponse>();
            return changeToCategoryResponceFormate;
        }

        public List<CategoryResponse> GetCategory()
        {
            var category = _categoryRepository.GetAll();
            var response = category.Adapt<List<CategoryResponse>>();
            return response;


        }
    }
}
