using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Training.DAL.Context;
using Training.DAL.Entities;
using TrainingStudent.Controllers;

namespace TrainingStudent.Helpers
{
    public class UserInformation
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly TableService tableService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SchoolingContext context;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserInformation(Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> roleManager, TableService tableService, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager, SchoolingContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.roleManager = roleManager;
            this.tableService = tableService;
            this.userManager = userManager;
            this.context = context;
            this.httpContextAccessor = httpContextAccessor;
        }
        public  async Task<List<string>> GetUserInformationAsync(string role)
        {
            const string permissionType = "Permission";
            var userInformation = new List<string>();

            if (role == null)
            {
                return userInformation; // Return an empty list if the role is null
            }

            var usersInRole = await userManager.GetUsersInRoleAsync(role);
            var roleObject = await roleManager.FindByNameAsync(role);

            if (roleObject == null || usersInRole == null)
            {
                return userInformation; // Return an empty list if the role or users in the role are not found
            }

            var roleClaims = await roleManager.GetClaimsAsync(roleObject);

            foreach (var user in usersInRole)
            {
                var userClaims = await userManager.GetClaimsAsync(user);
                var userPermissionClaims = userClaims.Where(c => c.Type == permissionType).Select(c => c.Value);

                foreach (var permission in userPermissionClaims)
                {
                    // Check if the permission claim exists in the role claims
                    if (roleClaims.Any(c => c.Type == permissionType && c.Value == permission))
                    {
                        userInformation.Add($"{user.UserName}: {permission}");
                    }
                }
            }

            return userInformation;
        }
        public async Task<bool> UserHasPermissionAsync(string permission)
        {
            var username = httpContextAccessor.HttpContext.User.Identity.Name;
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }

            var currentUser = await userManager.FindByNameAsync(username);
            if (currentUser == null)
            {
                return false;
            }

            var claims = await userManager.GetClaimsAsync(currentUser);
            return claims.Any(c => c.Type == "Permission" && c.Value == permission);
        }
        public async Task<List<string>> GetUserClaimsAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new List<string>(); // Return an empty list if user is not found
            }

            var claims = await userManager.GetClaimsAsync(user);
            return claims.Select(c => c.Value).ToList();
        }
    }
}
