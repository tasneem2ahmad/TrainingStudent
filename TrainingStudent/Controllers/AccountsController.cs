using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Data;
using System.Data.Entity;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Providers.Entities;
using Training.DAL.Context;
using Training.DAL.Entities;
using TrainingStudent.Helpers;
using TrainingStudent.Models;


namespace TrainingStudent.Controllers
{
    [AllowAnonymous]

    public class AccountsController : Controller
	{
		private object FormsAuthentication;
        private readonly SomeService someService;

        public Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> UserManager { get; }
		public SignInManager<ApplicationUser> SignInManager { get; }
		public IMapper Mapper { get; }
		public SchoolingContext Context { get; }
        public Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> RoleManager { get; }
        public ILogger<AccountsController> Logger { get; }

        public AccountsController(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper, SchoolingContext context,Microsoft.AspNetCore.Identity.RoleManager<IdentityRole>roleManager,ILogger<AccountsController>logger,SomeService someService)
		{
			UserManager = userManager;
			SignInManager = signInManager;
			Mapper = mapper;
			Context = context;
            RoleManager = roleManager;
            Logger = logger;
            this.someService = someService;
        }

		#region Register
		public async Task<IActionResult> Register()
		{
			//ViewData["Roles"] =await RoleManager.Roles.ToListAsync();
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel register)
		{
			

			if (ModelState.IsValid)
			{
				var registeruser = Mapper.Map<RegisterViewModel, ApplicationUser>(register);
				//registeruser.UserRoles= (ICollection<IdentityUserRole<string>>?)await RoleManager.Roles.ToListAsync();
				var result = await UserManager.CreateAsync(registeruser, register.Password);
				if (result.Succeeded)
				{
					await UserManager.AddToRoleAsync(registeruser, "User");

					return RedirectToAction(nameof(LogIn));
				}
				else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
					
					return View(register);
				}
				
			}
			return View(register);
		}
		#endregion
		#region Login
		[AllowAnonymous]
        public async Task<IActionResult> LogIn()
		{
			return View();
		}


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
					
                    var passwordValid = await UserManager.CheckPasswordAsync(user, model.Password);
                    if (passwordValid)
                    {
                        var result = await SignInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);
                        if (result.Succeeded)
                        {
                            var roles = await UserManager.GetRolesAsync(user);

                            Logger.LogInformation($"User {user.UserName} roles: {string.Join(", ", roles)}");

                            var claims = await UserManager.GetClaimsAsync(user);
                            Logger.LogInformation($"User {user.UserName} claims: {string.Join(", ", claims.Select(c => $"{c.Type} - {c.Value}"))}");
                            await someService.CheckUserPermissionsAsync(user.Id);

                            if (roles.Contains("Manager"))
                            {
                                return RedirectToAction("Index", "Home");
                            }
                            else if (roles.Contains("Admin"))
                            {
                                return RedirectToAction("Privacy", "Home");
                            }
                            else
                            {
                                return RedirectToAction("User", "Home");
                            }
                        }
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }


        #endregion
        #region Logout

        [Authorize]
        public async Task<IActionResult> Logout()
		{
            
            await SignInManager.SignOutAsync();
            

            // Clear session data (if applicable)
            HttpContext.Session.Clear();
            // Set a response header to prevent caching
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            // Check if the session is valid
            if (HttpContext.Session.GetString("UserId") == null)
            {
                // If session is invalid, redirect to the login page
                return RedirectToAction("Login");
            }
            // Redirect to the login page with a unique parameter to bypass caching
            return RedirectToAction("Login", "Accounts");
		}
		#endregion ForgetPassword
		public async Task<IActionResult> ForgetPassword()
		{
			return View();
		}
		[HttpPost]
        
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await UserManager.FindByEmailAsync(model.Email);
				if(user != null)
				{
					var token=await UserManager.GeneratePasswordResetTokenAsync(user);
					var resetpasswordlink = Url.Action("ResetPassword", "Accounts", new {Email = model.Email,token=token},Request.Scheme);
					var email = new Email()
					{
						Title = "Reset Password",
						To = model.Email,
						Body = resetpasswordlink
					};
					EmailSettings.SendEmail(email);
					
					return RedirectToAction(nameof(CompleteForgetPassword));
				}
				ModelState.AddModelError(string.Empty, "Email isnt Exist");
			}
			return View(model);
		}
		public async Task<IActionResult> CompleteForgetPassword()
		{
			return View();
		}
        #region ResetPassword
		public async Task<IActionResult> ResetPassword(string email,string token)
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await UserManager.FindByEmailAsync(model.Email);
				if (user != null)
				{
					var result = await UserManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
					//var resulthash=await UserManager.ChangePasswordAsync(user,model.NewPassword,model.Token);
					if(result.Succeeded) 
						return RedirectToAction(nameof(ResetPasswordDone));
					foreach(var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty,error.Description);
					}
				}
				ModelState.AddModelError(string.Empty, "Email doesnt exist");
			}
			return View();
		}
		public async Task<IActionResult> ResetPasswordDone()
		{
			return View();
		}
		#endregion
		#region Caching
		
		#endregion
		#region AccessDenied
		public async Task<IActionResult> AccessDenied()
		{
			return View();
		}
		#endregion
		#region
		#endregion
	}
}
