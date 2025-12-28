using KAshop.DAL.DTO.Request;
using KAshop.DAL.DTO.Response;
using KAshop.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
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
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthenticationService(UserManager<ApplicationUser> userManager,IConfiguration configuration,
             IEmailSender emailSender,SignInManager<ApplicationUser>signInManager

            )
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailSender = emailSender;
            _signInManager = signInManager;
        }
        public async Task<LoginResponse> LoginAsync(DAL.DTO.Request.LoginRequest request)
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

                if(await _userManager.IsLockedOutAsync(user))
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "account is loucked, try again later"
                    };
                }
                //the true in the parametrs counts how many fail attempt in writing password
                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);
                if (result.IsLockedOut)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "account is loucked,due to multiple trys "
                    };
                }

                else if (result.IsNotAllowed)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "plz confirm email "
                    };
                }
                else if (!result.Succeeded)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "password not correct "
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

        public async Task<RegesterResponce> RegisterAsync(DAL.DTO.Request.RegisterRequest request)
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
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                token = Uri.UnescapeDataString(token);
                var emailUrl = $"https://localhost:7175/api/auth/Account/ConfirmEmail?token={token}&userid={user.Id}";
                await _userManager.AddToRoleAsync(user, "User");
                await _emailSender.SendEmailAsync(user.Email, "welcom", $"<h1>welcome....{user.UserName} </h1> " +
                    $"<a href={emailUrl} >confirm email</a>");
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

        public async Task<bool> ConfirmEmailAsync(string token, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return false;
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded) return false;
            else
                return true;
        }


        private async Task<string> GenerateAccessToken(ApplicationUser user)
        {
            //what i want to store inside the token!! dont put the password in the token
            var roles = await _userManager.GetRolesAsync(user);
            var TokenClaims = new List<Claim>
            
            {
                 new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,string.Join(',',roles))
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecreatKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: TokenClaims,
                expires: DateTime.UtcNow.AddDays(5),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ForgetPasswordResponce> RequestPasswordReset(ForgetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user is null)
            {
                return new ForgetPasswordResponce()
                {
                    Success = false,
                    Message = "Email not found"
                };
            }

            var random = new Random();
            var code = random.Next(1000, 9999).ToString();

            user.CodeResetPassword = code;
            user.PasswordResetCodeExpiry = DateTime.UtcNow.AddMinutes(15);

            await _userManager.UpdateAsync(user);

            await _emailSender.SendEmailAsync(
                request.Email,
                "reset password",
                $"<p>code is {code}</p>"
            );
            return new ForgetPasswordResponce
            {
                Success = true,
                Message="code sent to your email"
            };

        }


        public async Task<ResetPasswordResponce> ResetPassword(DAL.DTO.Request.ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new ResetPasswordResponce()
                {
                    Success = false,
                    Message = "Email not found"
                };
            }

            else if (user.CodeResetPassword != request.Code)
            {
                return new ResetPasswordResponce()
                {
                    Success = false,
                    Message = "invalid code"
                };
            }


            else if (user.PasswordResetCodeExpiry <DateTime.UtcNow)
            {
                return new ResetPasswordResponce()
                {
                    Success = false,
                    Message = " code expired"
                };
            }


            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);

            if (!result.Succeeded)
            {
                return new ResetPasswordResponce()
                {
                    Success = false,
                    Message = "password reset error",
                    Errors=result.Errors.Select(e=>e.Description).ToList()
                };
            }

            await _emailSender.SendEmailAsync(
                request.Email,
                "changed  password",
                $"<p>your password is changed</p>"
            );
            return new ResetPasswordResponce
            {
                Success = true,
                Message = "password reset successfully"
            };

        }
    }

    
}
