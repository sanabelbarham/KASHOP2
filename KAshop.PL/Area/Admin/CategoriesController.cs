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

        [HttpPatch("")]
        public async Task< IActionResult >Create([FromBody] CategoryRequest request)
        {

            var CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var response = await _categorySerivce.CreateCategory(request);
            return Ok(new { message = _localizer["success"].Value });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromBody] CategoryRequest request)
        {
            var result = await _categorySerivce.UpdateCategoryAsync(id, request);
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

        [HttpPatch("toggle-status/{id}")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var result = await _categorySerivce.ToggleStatus(id);
            if (!result.Success)
            {
                if(result.Message.Contains("Not Found"))
                {
                    return NotFound(result);
                }
                return BadRequest(result);
            }
            return Ok(result);
        }

    }
}
