using KAshop.BLL.Service;
using KAshop.DAL.DTO.Request;
using KAshop.PL.Resourses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace KAshop.PL.Area.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : ControllerBase
    {

        public ICategoryService _categorySerivce;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CategoriesController(ICategoryService categories, IStringLocalizer<SharedResource> localizer)
        {
            _categorySerivce = categories;
            _localizer = localizer;
        }

        [HttpPost("")]
        public IActionResult Create(CategoryRequest request)
        {

            var CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var response = _categories.CreateCategory(request);
            return Ok(new { message = _localizer["success"].Value });
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteCategory([FromRoute] int id)
        {
            var result = await _categorySerivce.DeleteCategoryAsync(id);

            if (!result.Success)
            {
                if (result.Message.Contains("Not Found"))
                {
                    return NotFound(result);
                }

                return BadRequest(result);
            }

            return Ok(result);
        }


    }
}
