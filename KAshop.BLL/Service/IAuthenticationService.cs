using KAshop.DAL.DTO.Request;
using KAshop.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAshop.BLL.Service
{
   public interface IAuthenticationService
    {
        Task <RegesterResponce> RegisterAsync(RegisterRequest request);
        Task <LoginResponse> LoginAsync(LoginRequest request);
        Task<string> ConfirmEmailAsync(string email);


    }
}
