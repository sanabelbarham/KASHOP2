using KAshop.DAL.DTO.Request;
using KAshop.DAL.DTO.Response;
using KAshop.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KAshop.BLL.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;

        public AuthenticationService(UserManager<ApplicationUser> userManager,IConfiguration configuration,
             IEmailSender emailSender

            )
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailSender = emailSender;
        }
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user is null)
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
                    Message = "Login successfully",
                    AccessToken= await GenerateAccessToken(user)

                };

            }
            catch (Exception ex)
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
                await _emailSender.SendEmailAsync(user.Email, "welcom", $"<h1>welcome....{user.UserName}");
                return new RegesterResponce()
                {
                    Success = true,
                    Message = "success"
                };
            }
            catch (Exception ex)
            {
                return new RegesterResponce()
                {
                    Success = false,
                    Message = "an unexpected error",
                    Errors = new List<string> { ex.Message }

                };
            }

        }






        private async Task<string> GenerateAccessToken(ApplicationUser user)
        {
            //what i want to store inside the token!! dont put the password in the token
            var TokenClaims = new List<Claim>
            {
                 new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecreatKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: TokenClaims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        }

    
}
