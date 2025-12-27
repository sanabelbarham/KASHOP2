using KAshop.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAshop.DAL.DTO.Request
{
   public class ResetPasswordRequest:BaseResponce
    {
        public string Code { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }
      
    }
}
