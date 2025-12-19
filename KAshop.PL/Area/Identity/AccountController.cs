using KAshop.DAL.DTO.Request;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KAshop.BLL.Service;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;


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


        [HttpPost("login")]
        public async Task<IActionResult> Login(DAL.DTO.Request.LoginRequest request)
        {
            var result = await _authenticationService.LoginAsync(request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("Register")]
        public async Task <IActionResult> Register(DAL.DTO.Request.RegisterRequest request)
        {
            var result = await _authenticationService.RegisterAsync(request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("ConfirmEmail")]
        public async Task <IActionResult> ConfirmEmail(string token,string userId )
        {
            var result = await _authenticationService.ConfirmEmailAsync(token,userId);

          
            return Ok(result);
        }

        [HttpPost("SendCode")]
        public async Task<IActionResult> RequestPasswordReset(ForgetPasswordRequest request)
        {
            var result = await _authenticationService.RequestPasswordReset(request);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

    }
}
