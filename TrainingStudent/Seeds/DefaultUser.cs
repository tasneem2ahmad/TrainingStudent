using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TrainingStudent.Helpers;

namespace TrainingStudent.Seeds
{
    public static class DefaultUser
    {
        /// <summary>
        /// reflection method static,its parameter this. -------
        /// </summary>
        /// <param name="roleManager"></param>
        /// <param name="role"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public static async Task AddPermissionClaims(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = ClaimsandPermission.GeneratePermissionList(module);

            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(c => c.Type == "Permission" && c.Value == permission))
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
            }
        }
    }
}
