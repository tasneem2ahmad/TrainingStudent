using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using TrainingStudent.Helpers;
using Training.DAL.Context;
using System.Threading.Tasks;
using System.Collections.Generic;

public class AllPermission
{
    private readonly SchoolingContext _context;
    private readonly TableService _tableService;

    public AllPermission(SchoolingContext context, TableService tableService)
    {
        _context = context;
        _tableService = tableService;
    }

    public async Task<Dictionary<string, List<string>>> GetAllPermissionsAsync()
    {
        var modulePermissions = new Dictionary<string, List<string>>();

        // Assuming you have a method to get all modules from the database
        var modules = _tableService.GetTableNames();

        // Get all controllers in the assembly
        var controllers = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => typeof(Controller).IsAssignableFrom(type) && !type.IsAbstract)
            .ToList();

        foreach (var module in modules)
        {
            var moduleActionPermissions = new List<string>();

            foreach (var controller in controllers)
            {
                // Check if the controller name starts with the module name and ends with "Controller"
                if (!controller.Name.StartsWith(module) || !controller.Name.EndsWith("Controller"))
                {
                    continue;
                }

                var actions = controller.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)
                    .Where(m => !m.IsDefined(typeof(NonActionAttribute)));

                foreach (var action in actions)
                {
                    var authorizeAttributes = action.GetCustomAttributes<AuthorizeAttribute>();

                    foreach (var attribute in authorizeAttributes)
                    {
                        // Check if the policy is related to the current module
                        if (attribute.Policy != null && attribute.Policy.StartsWith($"Permissions.{module}."))
                        {
                            var permission = attribute.Policy.Substring($"Permissions.{module}.".Length);
                            var actionPermission = $"{action.Name}: {permission}";

                            if (!moduleActionPermissions.Contains(actionPermission))
                            {
                                moduleActionPermissions.Add(actionPermission);
                            }
                        }
                    }
                }
            }

            if (moduleActionPermissions.Any())
            {
                modulePermissions.Add(module, moduleActionPermissions);
            }
        }

        return modulePermissions;
    }

}
