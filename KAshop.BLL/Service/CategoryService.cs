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
using Microsoft.AspNetCore.Http.HttpResults;

namespace KAshop.BLL.Service
{
    public class CategoryService : ICategoryService
    {

        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryResponse> CreateCategory(CategoryRequest request)
        {
            var category = request.Adapt<Category>();
            _categoryRepository.CreateAsync(category);
            var changeToCategoryResponceFormate = category.Adapt<CategoryResponse>();
            return changeToCategoryResponceFormate;
        }

        public async Task<List<CategoryResponse>> GetCategory()
        {
            var category = await _categoryRepository.GetAllAsync();
            var response = category.Adapt<List<CategoryResponse>>();
            return response;


        }
        public async Task<BaseResponce> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _categoryRepository.FindByIdAsync(id);

                if (category is null)
                {
                    return new BaseResponce
                    {
                        Success = false,
                        Message = "Category Not Found"
                    };
                }

                await _categoryRepository.DeleteAsync(category);

                return new BaseResponce
                {
                    Success = true,
                    Message = "Category Deleted Succesfully"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponce
                {
                    Success = false,
                    Message = "Can't Delete Category",
                    Errors = new List<string> { ex.Message }
                };
            }

        }


        
        public async Task<BaseResponce> ToggleStatus(int id)
        {
            try
            {
                var category = await _categoryRepository.FindByIdAsync(id);
                if (category is null)
                {
                    return new BaseResponce
                    {
                        Success = false,
                        Message = "Category Not Found"
                    };
                }

                category.Status = category.Status == Status.Active ? Status.InActive : Status.Active;
                await _categoryRepository.UpdateAsync(category);
                return new BaseResponce
                {
                    Success = true,
                    Message = "Category Updated Successfully"
                };

            }
            catch (Exception ex)
            {
                return new BaseResponce
                {
                    Success = false,
                    Message = "Cant delete category",
                    Errors=new List<string> { ex.Message}
                };

            }
        }
public async Task<BaseResponce> UpdateCategoryAsync(int id, CategoryRequest request)
        {
            try
            {
                var category = await _categoryRepository.FindByIdAsync(id);
                if (category is null)
                {
                    return new BaseResponce
                    {
                        Success = false,
                        Message = "Category Not Found"
                    };
                }
                if (request.Translations != null)
                {
                    foreach (var tmslation in request.Translations)
                    {
                        var existing = category.Translations.FirstOrDefault(t => t.Language == tmslation.Language);

                        if (existing is not null)
                        {
                            existing.Name = tmslation.Name;
                        }
                        else
                        {
                            return new BaseResponce
                            {
                                Success = true,
                                Message = $" Language {tmslation.Language} not supported"
                            };
                        }
                    }
                }

                await _categoryRepository.UpdateAsync(category);
                return new BaseResponce
                {
                    Success = true,
                    Message = "Category Updated Successfully"
                };

            }
            catch (Exception ex)
            {
                return new BaseResponce
                {
                    Success = false,
                    Message = "CCan not update category ",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
