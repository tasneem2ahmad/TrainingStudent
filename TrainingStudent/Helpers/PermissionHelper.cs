using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TrainingStudent.Helpers
{
    public class PermissionHelper
    {

        public List<string> GetPermissionsFromControllers(List<string> controllerNames)
        {
            var permissions = new List<string>();

            // Get all types in the executing assembly
            var allTypes = Assembly.GetExecutingAssembly().GetTypes();

            // Iterate over controller names
            foreach (var controllerName in controllerNames)
            {
                // Find the controller type by name
                var controllerType = allTypes.FirstOrDefault(t => typeof(Controller).IsAssignableFrom(t) && t.Name == controllerName + "Controller");

                // Check if the controller type is found
                if (controllerType != null)
                {
                    // Get methods with authorize attributes
                    var methods = controllerType.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)
                                                .Where(m => m.GetCustomAttributes(typeof(AuthorizeAttribute), true).Any())
                                                .ToList();

                    // Iterate over methods
                    foreach (var method in methods)
                    {
                        // Get authorize attributes
                        var authorizeAttributes = method.GetCustomAttributes<AuthorizeAttribute>();
                        foreach (var authorizeAttribute in authorizeAttributes)
                        {
                            // Add permission to the list
                            var permission = !string.IsNullOrEmpty(authorizeAttribute.Policy) ? authorizeAttribute.Policy : $"{controllerType.Name.Replace("Controller", "")}.{method.Name}";
                            permissions.Add(permission);
                        }
                    }
                }
                else
                {
                    // Handle the case where the controller type is not found
                    Console.WriteLine($"Controller '{controllerName}Controller' not found.");
                }
            }

            return permissions.Distinct().ToList();
        }





    }

}
