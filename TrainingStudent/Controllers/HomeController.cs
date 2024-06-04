using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Web.Providers.Entities;
using Training.DAL.Context;
using Training.DAL.Entities;
using TrainingStudent.Helpers;
using TrainingStudent.Models;

namespace TrainingStudent.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SomeService someService;

        public HomeController(ILogger<HomeController> logger, SomeService someService)
        {
            _logger = logger;
            this.someService = someService;
        }
        
        [Authorize(Roles = "Manager")]

        
        public async Task< IActionResult> Index()
        {

            return View();
        }
        
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Privacy()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> User()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}