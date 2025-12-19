using KAshop.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAshop.DAL.Utils
{
    public class UserDataSeed : ISeedData
    {
        public UserManager<ApplicationUser> _UserManager; 


        public UserDataSeed( UserManager<ApplicationUser> userManager)
        {
            _UserManager = userManager;
        }

        public async Task DataSeed()
        {
            if(!await _UserManager.Users.AnyAsync())
            {
                var user1 = new ApplicationUser
                {
                    UserName = "sanabel",
                    Email = "sanabel@gmail.com",
                    FullName = "sanabel barham",
                    EmailConfirmed = true
                };
                var user2 = new ApplicationUser
                {
                    UserName = "malakoot",
                    Email = "malakoot@gmail.com",
                    FullName = "malakoot barham",
                    EmailConfirmed = true
                };
                var user3 = new ApplicationUser
                {
                    UserName = "sabri",
                    Email = "sabri@gmail.com",
                    FullName = "sabri barham",
                    EmailConfirmed = true
                };
                //the second part is the password
                await _UserManager.CreateAsync(user1, "Pass@123");
                await _UserManager.CreateAsync(user2, "Sbri@34g");
                await _UserManager.CreateAsync(user3, "AS34$5dd");


              await _UserManager.AddToRoleAsync(user1, "SuperAdmin");
                await _UserManager.AddToRoleAsync(user2, "Admin");
                await _UserManager.AddToRoleAsync(user3, "User");
            }


        }
       
    }
}
