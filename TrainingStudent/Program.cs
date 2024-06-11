using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System;
using System.Data;
using Training.BLL.Interfaces;
using Training.BLL.Repositories;
using Training.DAL.Context;
using Training.DAL.Entities;
using TrainingStudent.Filters;
using TrainingStudent.Helpers;
using TrainingStudent.Mappers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<SchoolingContext>(options =>
    
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<IAuthorizationPolicyProvider,PermissionPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler,PermissionAuthorizationHandler>();
builder.Services.AddScoped<IDepartmentRepository , DepartmentRepository>();
builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseStudentRepository, CourseStudentRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(m => m.AddProfile(new TeacherProfile()));
builder.Services.AddAutoMapper(m => m.AddProfile(new StudentProfile()));
builder.Services.AddAutoMapper(m => m.AddProfile(new UserProfile()));
builder.Services.AddAutoMapper(m => m.AddProfile(new RoleProfile()));
builder.Services.AddAutoMapper(m => m.AddProfile(new UserRoleProfile()));
builder.Services.AddAutoMapper(m => m.AddProfile(new CourseProfile()));
builder.Services.AddAutoMapper(m => m.AddProfile(new DepartmentProfile()));
builder.Services.AddAutoMapper(m => m.AddProfile(new CourseStudentProfile()));
builder.Services.AddScoped<TableService>();


//builder.Services.AddScoped<IGenericRepository, GenericRepositry>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
 .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
 {
	 options.Cookie.Name = "Identity.Application";
	 options.LoginPath = new PathString("/Accounts/Login");
     options.AccessDeniedPath = new PathString("/Accounts/AccessDenied");
 });

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 5;
    options.SignIn.RequireConfirmedAccount = false;
})
            .AddEntityFrameworkStores<SchoolingContext>()
            .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(TokenOptions.DefaultProvider)
            .AddRoles<IdentityRole>(); // Add roles
// Add authorization policies
builder.Services.AddAuthorization(options =>
{
    var permissionsType = typeof(Permissions);
    foreach (var nestedType in permissionsType.GetNestedTypes())
    {
        foreach (var field in nestedType.GetFields())
        {
            var permissionValue = field.GetValue(null)?.ToString();
            if (!string.IsNullOrEmpty(permissionValue))
            {
                options.AddPolicy(permissionValue, policy =>
                {
                    policy.Requirements.Add(new PermissionRequirement(permissionValue));
                });
            }
        }
    }
});





// Configure services
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("ManagerPolicy", policy =>
//    {
//        policy.RequireRole("Manager"); // Require users to have the "Manager" role
//    });
//    options.AddPolicy("AdminPolicy", policy =>
//    {
//        policy.RequireRole("Admin"); // Require users to have the "Admin" role
//    });
//    options.AddPolicy("UserPolicy", policy =>
//    {
//        policy.RequireRole("User"); // Require users to have the "Admin" role
//    });
//});
builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Accounts/Login";
    options.AccessDeniedPath = "/Accounts/AccessDenied";
    //options.SlidingExpiration = true;
});
builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.Zero;
});


builder.Services.AddSession(options =>
{
    options.Cookie.Name = "Identity.Application";
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Adjust the timeout as needed
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    //options.Cookie.Path = new PathString("/Account/Login");

});
//builder.Services.Configure<SecurityStampValidatorOptions>(options =>
//{
//    // enables immediate logout, after updating the user's stat.
//    options.ValidationInterval = TimeSpan.Zero;
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
};

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Accounts}/{action=Register}/{id?}");

app.Run();
