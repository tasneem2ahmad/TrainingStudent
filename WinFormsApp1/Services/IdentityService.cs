using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.DAL.Context;
using Training.DAL.Entities;

namespace WinFormsApp1.Services
{
    public class IdentityService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SchoolingContext context;

        public IdentityService(UserManager<ApplicationUser>userManager,RoleManager<IdentityRole>roleManager,SchoolingContext context)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.context = context;
        }
    }
}
