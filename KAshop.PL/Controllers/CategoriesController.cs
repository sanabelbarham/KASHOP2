using KAshop.DAL.Data;
using KAshop.BLL.Service;
using KAshop.DAL.DTO.Request;
using KAshop.DAL.DTO.Response;
using KAshop.DAL.Models;
using KAshop.PL.Resourses;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Authorization;

namespace KAshop.PL.Controllers
{
    [Route("api/controller/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ICategoryService _categoryService;


        public CategoriesController( IStringLocalizer<SharedResource> localizer,ICategoryService categoryService)
        {
        
            _localizer = localizer;
            _categoryService = categoryService;
        }
        [HttpGet("")]

        public IActionResult index()
        {
            var response = _categoryService.GetCategory();


            return Ok( new { message = _localizer["Success2"].Value, response });
        }
        [HttpPost("")]
        public IActionResult Create(CategoryRequest request)
        {

            _categoryService.CreateCategory(request);


            return Ok(new {
                message = _localizer["Success2"].Value});
        }


    }
}
