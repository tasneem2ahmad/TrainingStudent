namespace TrainingStudent.Helpers
{
    public class PermissionChecker
    {
        private readonly UserInformation userInformation;
        private readonly PermissionHelper permissionHelper;

        public PermissionChecker(UserInformation userInformation, PermissionHelper permissionHelper)
        {
            this.userInformation = userInformation;
            this.permissionHelper = permissionHelper;
        }

        public async Task<bool> HasUserRequiredPermissionsAsync(string userId, List<string> controllerPrefixes)
        {
            var requiredPermissions = permissionHelper.GetPermissionsFromControllers(controllerPrefixes);
            var userClaims = await userInformation.GetUserClaimsAsync(userId);

            foreach (var permission in requiredPermissions)
            {
                if (!userClaims.Contains(permission))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
