using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Web.Mvc.Filters;
using TrainingStudent.Helpers;

namespace TrainingStudent.Filters
{
    public class PermissionFilter : IAuthorizationFilter
    {
        private readonly string _permission;

        public PermissionFilter(string permission)
        {
            _permission = permission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var hasPermission = user.Claims.Any(c => c.Type == Permissions.Permission && c.Value == _permission);
            if (!hasPermission)
            {
                context.Result = new ForbidResult();
            }
        }
    }

    public class PermissionAttribute : TypeFilterAttribute
    {
        public PermissionAttribute(string permission) : base(typeof(PermissionFilter))
        {
            Arguments = new object[] { permission };
        }
    }
}
