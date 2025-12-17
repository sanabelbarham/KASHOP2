using KAshop.DAL.Data;
using KAshop.DAL.DTO.Repository;
using KAshop.DAL.DTO.Request;
using KAshop.DAL.DTO.Response;
using KAshop.DAL.Models;
using KAshop.PL.Resourses;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace KAshop.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CategoriesController( IStringLocalizer<SharedResource> localizer,ICategoryRepository categoryRepository)
        {
        
            _localizer = localizer;
            _categoryRepository = categoryRepository;
        }
        [HttpGet("")]

        public IActionResult index()
        {
            var categories = _categoryRepository.GetAll();
          
            var response = categories.Adapt<List<CategoryResponse>>();
            return Ok( new { message = _localizer["Success2"].Value, response });
        }
        [HttpPost("")]
        public IActionResult Create(CategoryRequest request)
        {

            var category = request.Adapt<Category>();
            _categoryRepository.Create(category);
         
            return Ok(new {
                message = _localizer["Success2"].Value});
        }


    }
}
