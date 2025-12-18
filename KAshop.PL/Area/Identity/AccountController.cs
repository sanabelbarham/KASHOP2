using KAshop.DAL.DTO.Request;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KAshop.BLL.Service;


namespace KAshop.PL.Area.Identity
{
    [Route("api/auth/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly BLL.Service.IAuthenticationService _authenticationService;
        public AccountController(BLL.Service.IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Register")]
        public async Task <IActionResult> Register(RegisterRequest request)
        {
            var result = await _authenticationService.RegisterAsync(request);
            return Ok(result);
        }
    }
}
