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
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if(user is null)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "Email not found",
                    };
                }
                var result = await _userManager.CheckPasswordAsync(user, request.Password);
                if (!result)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "invalid password"
                    };
                  
                }
                return new LoginResponse()
                {
                    Success = true,
                    Message = "Login successfully"

                };

            }
            catch(Exception ex)
            {
                return new LoginResponse()
                {
                    Success = false,
                    Message = "an unexpected error",
                    Errors = new List<string> { ex.Message }

                };
            }
        }

        public async Task<RegesterResponce> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var user = request.Adapt<ApplicationUser>();
                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    return new RegesterResponce()
                    {
                        Success = false,
                        Message = "User creation error",
                        Errors = result.Errors.Select(e => e.Description).ToList()

                    };
                }

                await _userManager.AddToRoleAsync(user, "User");
                return new RegesterResponce()
                {
                    Success = true,
                    Message = "success"
                };
            }
            catch(Exception ex)
            {
                return new RegesterResponce()
                {
                    Success = false,
                    Message = "an unexpected error",
                    Errors = new List<string> { ex.Message}

                };
            }

        }
    }
}
