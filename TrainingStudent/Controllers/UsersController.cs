using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using Training.BLL.Interfaces;
using Training.DAL.Context;
using Training.DAL.Entities;
using TrainingStudent.Helpers;
using TrainingStudent.Models;

namespace TrainingStudent.Controllers
{
    [Authorize(Roles = "Manager")]
    public class UsersController : Controller
    {
        
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SchoolingContext context;
        private readonly ILogger logger;
        private readonly TableService tableService;

        public UsersController(UserManager<ApplicationUser>userManager , IMapper mapper,RoleManager<IdentityRole> roleManager,SchoolingContext context,ILogger<UsersController> logger,TableService tableService)
        {
			this.userManager = userManager;
            this.mapper = mapper;
            this.roleManager = roleManager;
            this.context = context;
            this.logger = logger;
            this.tableService = tableService;
        }
        public async Task<IActionResult> Index(string SearchValue)
        {
            ViewData["Roles"] = roleManager.Roles.ToList();
            
            var userss = userManager.Users .ToList();
            
            List<UserViewModel> users = new List<UserViewModel>();
            foreach(var user in userss)
            {
                
               var userlist = mapper.Map<ApplicationUser, UserViewModel>(user);
                

                users.Add(userlist);
            }
            if (string.IsNullOrEmpty(SearchValue))
            {
                
                return View(users );
            }
            else
            {
                var userr=await userManager.FindByNameAsync(SearchValue);
                var user=mapper.Map<ApplicationUser, UserViewModel>(userr);
                return View(new List<UserViewModel> (){user});
            }
        }
        public async Task<IActionResult> Details(string id,string ViewName="Details")
        {
            if (id == null)
                return NotFound();
            var user = await userManager.FindByIdAsync(id);
            ViewData["Roles"] = roleManager.Roles.ToList();


            var usermodel =mapper.Map<ApplicationUser, UserViewModel> (user);
            
            return View(ViewName, usermodel);
        }
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            
            ViewData["Roles"] = roleManager.Roles.ToList();

            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await userManager.GetRolesAsync(user);
            string userRole = userRoles.FirstOrDefault() ?? "User";

            var model = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsAgree = user.IsAgree,
                Role = userRole,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UserViewModel updatedUser)
        {
            if (id == null || updatedUser == null)
            {
                return BadRequest();
            }

            ViewData["Roles"] = roleManager.Roles.ToList();

            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Update user properties
                user.UserName = updatedUser.UserName;
                user.Email = updatedUser.Email;
                user.PhoneNumber = updatedUser.PhoneNumber;
                user.IsAgree = updatedUser.IsAgree;
                user.Role = updatedUser.Role;
                // Update user roles in ASP.NET Identity
                var userRoles = await userManager.GetRolesAsync(user);
                var currentRole = userRoles.FirstOrDefault();
                if (!string.IsNullOrEmpty(currentRole))
                {
                    await userManager.RemoveFromRoleAsync(user, currentRole);
                }
                if (!string.IsNullOrEmpty(updatedUser.Role))
                {
                    await userManager.AddToRoleAsync(user, updatedUser.Role);
                }

                // Update user roles in ASP.NET UserRoles
                var userRolesInUserRoleTable = await userManager.GetRolesAsync(user);
                var currentRoleInUserRoleTable = userRolesInUserRoleTable.FirstOrDefault();
                if (!string.IsNullOrEmpty(currentRoleInUserRoleTable))
                {
                    await userManager.RemoveFromRoleAsync(user, currentRoleInUserRoleTable);
                }
                
                   var updateResult= await userManager.AddToRoleAsync(user, updatedUser.Role);
                    context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                // Save changes to user
                //var updateResult = await userManager.UpdateAsync(user);
                if (updateResult.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(updatedUser);
        }




        public async Task<IActionResult> Delete(string? id)
        {
            ViewData["Roles"] =  roleManager.Roles.ToList();
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete([FromRoute] string? id, UserViewModel deletedUser)
        {
            ViewData["Roles"] =  roleManager.Roles.ToList();
            var user = await userManager.FindByIdAsync(deletedUser.Id);
			if (id != deletedUser.Id)
                return BadRequest();

			try
			{
				await userManager.DeleteAsync(user);
				return RedirectToAction("Index");
			}
			catch
			{
                return View(deletedUser);
            }


			
		}
        public async Task<IActionResult> ManagePermissions(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Get user's roles
            var userRoles = await userManager.GetRolesAsync(user);

            // Get all permissions associated with user's roles
            var roleClaims = new List<Claim>();
            foreach (var role in userRoles)
            {
                var roleObject = await roleManager.FindByNameAsync(role);
                if (roleObject != null)
                {
                    var claims = await roleManager.GetClaimsAsync(roleObject);
                    roleClaims.AddRange(claims);
                }
            }

            // Get user-specific claims
            var userClaims = await userManager.GetClaimsAsync(user);

            // Convert role claims and user-specific claims to CheckboxViewModels
            var allPermissions = ClaimsandPermission.GetAllPermission(tableService)
                .Select(p => new CheckboxViewModel
                {
                    DisplayValue = p,
                    IsChecked = userClaims.Any(c => c.Type == "Permission" && c.Value == p)
                }).ToList();

            var viewModel = new PermissionUserFormViewModel
            {
                Id = userId,
                Name = user.UserName,
                Claim = allPermissions
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManagePermissions(PermissionUserFormViewModel model)
        {
            if (model == null || model.Id == null)
            {
                return BadRequest();
            }

            var user = await userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            // Get current user-specific claims
            var userClaims = await userManager.GetClaimsAsync(user);

            // Remove existing user-specific permission claims
            foreach (var claim in userClaims.Where(c => c.Type == "Permission").ToList())
            {
                await userManager.RemoveClaimAsync(user, claim);
            }

            // Add new user-specific claims based on selected permissions
            var selectedClaims = model.Claim
                .Where(c => c.IsChecked)
                .Select(c => c.DisplayValue);

            foreach (var claimValue in selectedClaims)
            {
                await userManager.AddClaimAsync(user, new Claim("Permission", claimValue));
            }

            // Log user claims for debugging
            var updatedUserClaims = await userManager.GetClaimsAsync(user);
            logger.LogInformation($"User {user.UserName} updated claims: {string.Join(", ", updatedUserClaims.Select(c => $"{c.Type} - {c.Value}"))}");

            return RedirectToAction(nameof(Index));
        }










    }
}
