using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows.Forms;
using Training.DAL.Context;
using Training.DAL.Entities;
using WinFormsApp1.Services;

namespace WinFormsApp1
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var host = CreateHostBuilder().Build();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var identityService = services.GetRequiredService<IdentityService>();

                ApplicationConfiguration.Initialize();
                //Application.Run(new MainForm(identityService)); // Ensure MainForm accepts IdentityService
            }
        }

        static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Configure DbContext
                    services.AddDbContext<SchoolingContext>(options =>
                        options.UseSqlServer("DefaultConnection")); // Add your connection string here

                    // Configure Identity
                    //services.AddIdentity<ApplicationUser, IdentityRole>()
                    //    .AddEntityFrameworkStores<SchoolingContext>()
                    //    .AddDefaultTokenProviders();

                    // Register the IdentityService
                    services.AddTransient<IdentityService>();
                });
    }
}
