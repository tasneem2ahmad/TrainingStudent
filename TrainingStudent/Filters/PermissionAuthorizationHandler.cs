using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using TrainingStudent.Helpers;

namespace TrainingStudent.Filters
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly ILogger<PermissionAuthorizationHandler> _logger;

        public PermissionAuthorizationHandler(ILogger<PermissionAuthorizationHandler> logger)
        {
            _logger = logger;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User == null)
            {
                _logger.LogWarning("User is null in authorization context.");
                return Task.CompletedTask;
            }

            _logger.LogInformation($"Checking permissions for requirement: {requirement.Permission}");

            // Check only permission claims
            var hasPermission = context.User.Claims.Any(c =>
                c.Type == Permissions.Permission &&
                c.Value == requirement.Permission);

            if (hasPermission)
            {
                _logger.LogInformation($"User has permission: {requirement.Permission}");
                context.Succeed(requirement);
            }
            else
            {
                _logger.LogWarning($"User does not have permission: {requirement.Permission}");
            }

            return Task.CompletedTask;
        }
    }
}
