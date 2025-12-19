using KAshop.BLL.Service;
using KAshop.DAL.DTO.Request;
using KAshop.PL.Resourses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KAshop.PL.Area.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {

        public ICategoryService _categories;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CategoriesController(ICategoryService categories, IStringLocalizer<SharedResource> localizer)
        {
            _categories = categories;
            _localizer = localizer;
        }

        [HttpPost("")]
        public IActionResult Create(CategoryRequest request)
        {
            var response = _categories.CreateCategory(request);
            return Ok(new { message = _localizer["success"].Value });
        }
    }
}
