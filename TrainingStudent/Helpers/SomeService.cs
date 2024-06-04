using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Training.DAL.Entities;

namespace TrainingStudent.Helpers
{
    public class SomeService
    {
        private readonly PermissionChecker permissionChecker;
        private readonly TableService tableService;
        private readonly PermissionHelper permissionHelper;
        private readonly UserManager<ApplicationUser> userManager;

        public SomeService(PermissionChecker permissionChecker, TableService tableService, PermissionHelper permissionHelper, UserManager<ApplicationUser> userManager)
        {
            this.permissionChecker = permissionChecker;
            this.tableService = tableService;
            this.permissionHelper = permissionHelper;
            this.userManager = userManager;
        }

        public async Task<List<string>> CheckUserPermissionsAsync(string userId)
        {
            var controllerPrefixes = tableService.GetTableNames().ToList();
            var hasPermissions = await permissionChecker.HasUserRequiredPermissionsAsync(userId, controllerPrefixes);

            if (hasPermissions)
            {
                Console.WriteLine("User has all required permissions.");
                return await GetAccessibleActionsAsync(userId, controllerPrefixes);
            }
            else
            {
                Console.WriteLine("User does not have all required permissions.");
                return new List<string>(); // Returning an empty list to indicate no access
            }
        }

        private async Task<List<string>> GetAccessibleActionsAsync(string userId, List<string> controllerPrefixes)
        {
            var allPermissions = permissionHelper.GetPermissionsFromControllers(controllerPrefixes);
            var userClaims = await userManager.GetClaimsAsync(await userManager.FindByIdAsync(userId));
            var accessibleActions = new List<string>();

            foreach (var permission in allPermissions)
            {
                if (userClaims.Any(claim => claim.Value == permission))
                {
                    accessibleActions.Add(permission);
                }
            }

            return accessibleActions;
        }
    }
}
