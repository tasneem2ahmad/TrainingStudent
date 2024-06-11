using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Training.BLL.Interfaces;
using Training.DAL.Entities;
using TrainingStudent.Filters;
using TrainingStudent.Helpers;
using TrainingStudent.Models;

namespace TrainingStudent.Controllers
{
    [Authorize]

    public class CoursesController : Controller
    {
        private readonly IMapper mapper;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager;

        private readonly TableService tableService;

        public IUnitOfWork UnitOfWork { get; }
       

        public CoursesController( IUnitOfWork unitOfWork,IMapper mapper, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser>userManager,TableService tableService)
        {
            UnitOfWork = unitOfWork;
            this.mapper = mapper;
            this.userManager = userManager;

            this.tableService = tableService;
        }


        [Authorize(Policy = Permissions.Courses.View)]
        public async Task< IActionResult> Index(string SearchValue)
        {
            ViewData["department"]=await UnitOfWork.DepartmentRepository.GetAll();
            if (string.IsNullOrEmpty(SearchValue))
            {
                IEnumerable<CourseViewModel> model = mapper.Map<IEnumerable<Course>, IEnumerable<CourseViewModel>>(await UnitOfWork.CourseRepository.GetAll());
                return View(model);
            }
            else
            {
                var model = mapper.Map<IEnumerable<Course>, IEnumerable<CourseViewModel>>(await UnitOfWork.CourseRepository.SearchCourse(SearchValue));
                return View(model);
            }
        }
        [Authorize(Policy = Permissions.Courses.Details)]
        public async Task<IActionResult>Details(int? id, string ViewName = "Details")
        {
            if (id == null)
                return NotFound();
            ViewData["department"] = await UnitOfWork.DepartmentRepository.GetAll();
            var Coursedetails=await UnitOfWork.CourseRepository.Get(id);
            var course =  mapper.Map<Course, CourseViewModel>(Coursedetails);
            return View(ViewName, course);
        }
        [Authorize(Policy = Permissions.Courses.Create)]
        public async Task<IActionResult> Create()
        {
            ViewData["department"] = await UnitOfWork.DepartmentRepository.GetAll();
            return View();
        }
        [HttpPost]

        [Authorize(Policy = Permissions.Courses.Create)]
        public async Task<IActionResult> Create(CourseViewModel model)
        {
            ViewData["department"] = await UnitOfWork.DepartmentRepository.GetAll();
            if (ModelState.IsValid)
            {
                var coursemodel =mapper.Map<CourseViewModel,Course>(model);
                await UnitOfWork.CourseRepository.Add(coursemodel);
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
            Console.WriteLine("SaveCreateNewCourse action isnot executing...");
            return View("Create", model);
            
        }
        [Authorize(Policy = Permissions.Courses.Edit)]
        public async Task<IActionResult>Edit(int id)
        {
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Courses.Edit)]
        public async Task<IActionResult> Edit(int id,CourseViewModel model)
        {
            if (model.Id == null)
                return BadRequest();
            if (ModelState.IsValid)
            {
                var updatedCourse=mapper.Map<CourseViewModel,Course>(model);
                await UnitOfWork.CourseRepository.Update(updatedCourse);
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
            Console.WriteLine("SaveEditCourse action isnot executing...");
            return View("Edit", model);
        }
        [Authorize(Policy = Permissions.Courses.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Courses.Delete)]
        public async Task<IActionResult> Delete(int id,CourseViewModel model)
        {
            if(model.Id==null)
                return NotFound();
            if (ModelState.IsValid)
            {
                

                var deletedCourse=mapper.Map<CourseViewModel,Course>(model);
                await UnitOfWork.CourseRepository.Delete(deletedCourse);
                return RedirectToAction(nameof(Index));
            }
            foreach (var modelStateKey in ModelState.Keys)
            {
                var modelStateVal = ModelState[modelStateKey];
                foreach (var error in modelStateVal.Errors)
                {
                    Console.WriteLine($"{modelStateKey}: {error.ErrorMessage}");
                }

            }
            Console.WriteLine("SaveDeleteCourse action isnot executing...");
            return View("Delete", model);
        }

    }
}
