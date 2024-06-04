using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Training.BLL.Interfaces;
using Training.BLL.Repositories;
using Training.DAL.Context;
using Training.DAL.Entities;
using Training.DAL.Migrations;
using TrainingStudent.Filters;
using TrainingStudent.Helpers;
using TrainingStudent.Models;

namespace TrainingStudent.Controllers
{
    [Authorize]
    public class TeachersController : Controller
    {
        public IUnitOfWork UnitOfWork { get; }
        private readonly IMapper mapper;

        public TeachersController(IUnitOfWork UnitOfWork,IMapper mapper)
        {
            
            this.UnitOfWork = UnitOfWork;
            this.mapper = mapper;
        }
        [Authorize(Permissions.Teachers.View)]
        public async Task<IActionResult> Index(string SearchValue)
        {
            if (string.IsNullOrEmpty(SearchValue)) { 
                var employees = mapper.Map<IEnumerable<Teacher>, IEnumerable<TeacherViewModel>>(await UnitOfWork.TeacherRepository.GetAll());
                return View(employees);
            }
            else { 
                var employees = mapper.Map<IEnumerable<Teacher>, IEnumerable<TeacherViewModel>>(await UnitOfWork.TeacherRepository.SearchTeacher(SearchValue));
                
                return View(employees);
            }
            
        }

        [Authorize(Permissions.Teachers.Create)]
        public async Task<IActionResult> Create()
        {
            ViewData["department"] = await UnitOfWork.DepartmentRepository.GetAll();
            ViewData["Course"]=await UnitOfWork.CourseRepository.GetAll();  
            return View();
        }
        [HttpPost]
        [Authorize(Permissions.Teachers.Create)]
        public async Task<IActionResult> Create(TeacherViewModel teacher)
        {
            ViewData["department"] =await UnitOfWork.DepartmentRepository.GetAll();
            ViewData["Course"] = await UnitOfWork.CourseRepository.GetAll();
            if (teacher == null)
            {
                // Handle the case where employee is null, perhaps by returning a bad request or an error view
                return BadRequest("Employee data is null.");
            }
            if (!ModelState.IsValid)
            {
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var modelStateVal = ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        Console.WriteLine($"{modelStateKey}: {error.ErrorMessage}");
                    }

                }
                Console.WriteLine("SaveCreateNewemployee action isnot executing...");
                ViewData["department"] =await UnitOfWork.DepartmentRepository.GetAll();
                ViewData["Course"] = await UnitOfWork.CourseRepository.GetAll();
                return View("Create", teacher);
            }
            //the error that when you click on submit the upload input cleared you goes to teacherviewmodel.cs and make property of imagename nullable
            teacher.ImageName=DocumentSetting.UploadFile(teacher.Image, "Imgs");
            var teach = mapper.Map<TeacherViewModel, Teacher>(teacher);
            
            await UnitOfWork.TeacherRepository.Add(teach);
            return RedirectToAction("Index");

        }
        [Authorize(Permissions.Teachers.Details)]
        public async Task<IActionResult> Details(int? id,string ViewName="Details")
        {
            if (id == null)
                return NotFound();
            ViewData["department"] = await UnitOfWork.DepartmentRepository.GetAll();
            ViewData["Course"] = await UnitOfWork.CourseRepository.GetAll();
            var teachers = await UnitOfWork.TeacherRepository.Get(id);
            var teach = mapper.Map< Teacher, TeacherViewModel>(teachers);
            return View(ViewName, teach);
        }
        [Authorize(Permissions.Teachers.Edit)]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["department"] =await UnitOfWork.DepartmentRepository.GetAll();
            ViewData["Course"] = await UnitOfWork.CourseRepository.GetAll();
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]//no one can call edit unless using its form 
        [Authorize(Permissions.Teachers.Edit)]
        public async Task<IActionResult> Edit([FromRoute] int? id, TeacherViewModel teacher)
        {


            ViewData["Course"] = await UnitOfWork.CourseRepository.GetAll();
            ViewData["department"] = await UnitOfWork.DepartmentRepository.GetAll();
            if (id != teacher.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    if(teacher != null) {
                        
                        var teach = mapper.Map<TeacherViewModel, Teacher>(teacher);
                        if (teacher.Image != null)
                        {
                            // Delete the old image
                            DocumentSetting.DeleteFile(teach.ImageName, "Imgs");

                            // Update the file name with the new image
                            teach.ImageName = DocumentSetting.UploadFile(teacher.Image, "Imgs");
                        }
                        await UnitOfWork.TeacherRepository.Update(teach);
                        return RedirectToAction("Index");
                    }
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return View(teacher);
                }
            }
            return View(teacher);
        }
        [Authorize(Permissions.Teachers.Delete)]
        public async Task<IActionResult> Delete(int? id)
        {
            ViewData["department"] = await UnitOfWork.DepartmentRepository.GetAll();
            ViewData["Course"] = await UnitOfWork.CourseRepository.GetAll();

            return await Details(id, "Delete");
        }
        
        [HttpPost] //[HttpDelete] in API
        [ValidateAntiForgeryToken]//no one can call edit unless using its form 
        [Authorize(Permissions.Teachers.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int? id, TeacherViewModel teacher)
        {
            ViewData["Course"] = await UnitOfWork.CourseRepository.GetAll();
            ViewData["department"] = await UnitOfWork.DepartmentRepository.GetAll();

            if (id != teacher.Id)
                return BadRequest(); //status code 400
                                     // Get referenced tables dynamically
            
            try
            {
                var teach = mapper.Map<TeacherViewModel, Teacher>(teacher);
                DocumentSetting.DeleteFile(teach.ImageName, "Imgs");
                await UnitOfWork.TeacherRepository.Delete(teach);
                return RedirectToAction("Index");
            }
            
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] = "An error occurred while trying to delete the teacher.";
                return RedirectToAction("Index");
            }
            
            

        }
    }
}
