using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Entity;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using Training.BLL.Repositories;
using Training.DAL.Context;
using Training.DAL.Entities;
using TrainingStudent.Filters;
using TrainingStudent.Helpers;
using TrainingStudent.Models;

namespace TrainingStudent.Controllers
{
    
    [Authorize(Roles = "Manager")]
    
    public class RolesController : Controller
    {
        private readonly Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;
        private readonly TableService tableService;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager;
        private readonly ILogger<RolesController> logger;
        private readonly SchoolingContext context;
        private readonly IHttpContextAccessor httpContextAccessor;

        public RolesController(Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> roleManager, IMapper mapper, TableService tableService, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser>userManager,ILogger<RolesController>_logger, SchoolingContext context,IHttpContextAccessor httpContextAccessor)
        {
            this.roleManager = roleManager;
            this.mapper = mapper;
            this.tableService = tableService;
            this.userManager = userManager;
            logger = _logger;
            this.context = context;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            //var roles = await roleManager.Roles.ToListAsync(); // Ensure to use ToListAsync() if you're working with Entity Framework Core
            //List<RoleViewModel> rolesViewModel = new List<RoleViewModel>();
            //foreach(var item in roles)
            //{
            //    var roleUser=mapper.Map<IdentityRole,RoleViewModel>(item);
            //    rolesViewModel.Add(roleUser);
            //}
            
            var roles = roleManager.Roles.ToList();
            var permission=new List<string>();
            var rolesViewModel = mapper.Map<List<RoleViewModel>>(roles);
            foreach (var item in roles)
            {
                var permissionroles = roleManager.GetRoleIdAsync(item);
                permission.Add(permissionroles.ToString());
            }
            ViewData["permission"] = permission;
            
            
            return View(rolesViewModel);
        }
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create(RoleViewModel role)
        {
            if(role==null)
            {
                return BadRequest();
            }
            if(ModelState.IsValid)
            { 
                
                var roleModel =mapper.Map<RoleViewModel, IdentityRole>(role);
                roleModel.NormalizedName = role.Name.ToUpper();
                await roleManager.CreateAsync(roleModel);
                return RedirectToAction("Index");
            }
            foreach (var modelStateKey in ModelState.Keys)
            {
                var modelStateVal = ModelState[modelStateKey];
                foreach (var error in modelStateVal.Errors)
                {
                    Console.WriteLine($"{modelStateKey}: {error.ErrorMessage}");
                }

            }
            Console.WriteLine("SaveCreateNewRole action isnot executing...");
            return View("Create", role);
            
        }
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> Details(string? id, string ViewName = "Details")
        {
            if (id == null)
                return NotFound();
            var role=await roleManager.FindByIdAsync(id);
            var maprole = mapper.Map<IdentityRole, RoleViewModel>(role);
            return View(ViewName, maprole);
        }
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id,"Edit");
        }
        [HttpPost]
       // [Authorize(Roles = "Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModel role)
        {
            if (id != role.Id)
                return BadRequest();
            var findrole = await roleManager.FindByIdAsync(role.Id);


            if (ModelState.IsValid)
            {
                findrole.Name = role.Name;
                var result = await roleManager.UpdateAsync(findrole);

                if (!result.Succeeded)
                {
                    // If the update fails, add model errors and return the view
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(role);
                }

                return RedirectToAction("Index");
            }
            return View(role);
        }
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult>Delete(string id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        //[Authorize(Roles = "Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute]string id,RoleViewModel role)
        {
            if(id != role.Id)
                return BadRequest();
            var delrole = await roleManager.FindByIdAsync(role.Id);
            if (ModelState.IsValid)
            {
                
                var result=await roleManager.DeleteAsync(delrole);
                if (!result.Succeeded)
                {
                    // If the update fails, add model errors and return the view
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(role);
                }

                return RedirectToAction("Index");
            }
            return View(role);
        }
        public async Task<IActionResult> ManagePermissions(string roleId)
        {
            const string permissionType = "Permission";
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound();
            }

            var roleClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = ClaimsandPermission.GetAllPermission(tableService)
                                  .Select(p => new CheckboxViewModel { DisplayValue = p }).ToList();

            foreach (var permission in allPermissions)
            {
                if (roleClaims.Any(c => c.Type == permissionType && c.Value == permission.DisplayValue))
                {
                    permission.IsChecked = true;
                }
            }

            var viewModel = new PermissionFormViewModel
            {
                RoleId = roleId,
                RoleName = role.Name,
                RoleClaim = allPermissions
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManagePermissions(PermissionFormViewModel model)
        {
            if (model == null || model.RoleId == null)
            {
                return BadRequest();
            }

            var role = await roleManager.FindByIdAsync(model.RoleId);
            if (role == null)
            {
                return NotFound();
            }

            var roleClaims = await roleManager.GetClaimsAsync(role);

            // Remove existing role claims
            foreach (var claim in roleClaims)
            {
                await roleManager.RemoveClaimAsync(role, claim);
            }

            // Add new role claims
            var selectedClaims = model.RoleClaim.Where(c => c.IsChecked).ToList();
            foreach (var claim in selectedClaims)
            {
                await roleManager.AddClaimAsync(role, new Claim("Permission", claim.DisplayValue));
            }

            // Update user claims for all users in this role
            var usersInRole = await userManager.GetUsersInRoleAsync(role.Name);
            foreach (var user in usersInRole)
            {
                var userClaims = await userManager.GetClaimsAsync(user);

                // Remove existing user claims related to permissions
                foreach (var claim in userClaims.Where(c => c.Type == "Permission").ToList())
                {
                    await userManager.RemoveClaimAsync(user, claim);
                }

                // Add new user claims based on updated role claims
                foreach (var claim in selectedClaims)
                {
                    await userManager.AddClaimAsync(user, new Claim("Permission", claim.DisplayValue));
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<string> GetUserInformationAsync()
        {
            const string permissionType = "Permission";

            // Get the currently logged-in user's username
            var username = User.Identity.Name;
            if (string.IsNullOrEmpty(username))
            {
                return "No user is logged in."; // Return an informative message if no user is logged in
            }

            // Get the currently logged-in user object
            var currentUser = await userManager.FindByNameAsync(username);
            if (currentUser == null)
            {
                return "User not found."; // Return an informative message if the user is not found
            }

            // Get the user's ID
            var userId = currentUser.Id;

            // Get the roles of the currently logged-in user
            var roles = await userManager.GetRolesAsync(currentUser);
            if (roles == null || roles.Count == 0)
            {
                return "No roles assigned to the user."; // Return an informative message if the user has no roles
            }

            // Assuming the primary role is the first role in the list
            var primaryRole = roles.First();

            // Get user claims
            var userClaims = await userManager.GetClaimsAsync(currentUser);
            var userPermissionClaims = userClaims.Where(c => c.Type == permissionType).Select(c => c.Value).ToList();

            // Check role claims and filter user permissions for the primary role
            var validPermissions = new List<string>();
            var roleObject = await roleManager.FindByNameAsync(primaryRole);
            if (roleObject != null)
            {
                var roleClaims = await roleManager.GetClaimsAsync(roleObject);
                foreach (var permission in userPermissionClaims)
                {
                    if (roleClaims.Any(c => c.Type == permissionType && c.Value == permission))
                    {
                        validPermissions.Add(permission);
                    }
                }
            }

            // Create the user information string
            var userInfo = new StringBuilder();
            userInfo.AppendLine($"User: {currentUser.UserName}");
            userInfo.AppendLine($"UserID: {userId}");
            userInfo.AppendLine($"Primary Role: {primaryRole}");
            userInfo.AppendLine("Permissions: " + string.Join(", ", validPermissions));

            return userInfo.ToString();
        }
        

















    }
}
