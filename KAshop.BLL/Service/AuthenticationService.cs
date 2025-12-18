using KAshop.DAL.DTO.Request;
using KAshop.DAL.DTO.Response;
using KAshop.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAshop.BLL.Service
{
   public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthenticationService(UserManager <ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            throw new NotImplementedException(); 
        }

        public async Task<RegesterResponce> RegisterAsync(RegisterRequest request)
        {
            var user = request.Adapt<ApplicationUser>();
            var result= await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return new RegesterResponce()
                {
                    Message = "error"
                };
            }

            await _userManager.AddToRoleAsync(user, "User");
            return new RegesterResponce()
            {
                Message = "success"
            };

        }
    }
}
