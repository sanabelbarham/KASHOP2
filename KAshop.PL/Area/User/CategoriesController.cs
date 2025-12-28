using KAshop.BLL.Service;
using KAshop.DAL.DTO.Request;
using KAshop.PL.Resourses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KAshop.PL.Area.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        public ICategoryService _categories;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CategoriesController(ICategoryService categories,IStringLocalizer<SharedResource> localizer)
        {
            _categories = categories;
            _localizer = localizer;
        }

     

        [HttpGet("")]
        public async Task< IActionResult> Index()
        {
            var getData = await _categories.GetCategory();
            return Ok(new { message = _localizer["success"].Value, getData });
        }

       

    }
}
