using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using Training.BLL.Interfaces;
using Training.DAL.Entities;
using TrainingStudent.Filters;
using TrainingStudent.Helpers;
using TrainingStudent.Models;

namespace TrainingStudent.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        public IUnitOfWork UnitOfWork { get; }
        private readonly IMapper mapper;

        public StudentsController(IUnitOfWork UnitOfWork, IMapper mapper)
        {

            this.UnitOfWork = UnitOfWork;
            this.mapper = mapper;
        }
        [Authorize(Permissions.Students.View)]
        public async Task<IActionResult> Index(string SearchValue)
        {
            ViewData["department"] = await UnitOfWork.DepartmentRepository.GetAll();
            if (string.IsNullOrEmpty(SearchValue))
            {
                var students = mapper.Map<IEnumerable<Student>, IEnumerable<StudentViewModel>>(await UnitOfWork.StudentRepository.GetAll());
                return View(students);
            }
            else
            {
                var searchStudent= mapper.Map<IEnumerable<Student>, IEnumerable<StudentViewModel>>(await UnitOfWork.StudentRepository.SearchStudent(SearchValue));
                return View(searchStudent);
            }
        }
        [Authorize(Permissions.Students.Create)]
        public async Task<IActionResult> Create()
        {
            ViewData["department"] = await UnitOfWork.DepartmentRepository.GetAll();
            return View();
        }
        [HttpPost]
        [Authorize(Permissions.Students.Create)]
        public async Task<IActionResult> Create(StudentViewModel student)
        {
            ViewData["department"] = await UnitOfWork.DepartmentRepository.GetAll();
            if (student == null)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
              var newStudent=mapper.Map<StudentViewModel,Student>(student);
                await UnitOfWork.StudentRepository.Add(newStudent);
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
            ViewData["department"] = await UnitOfWork.DepartmentRepository.GetAll();
            Console.WriteLine("SaveCreateNewStudent action isnot executing...");
            return View("Create", student);
        }
        [Authorize(Permissions.Students.Details)]
        public async Task<IActionResult> Details(int? id,string ViewName="Details")
        {
            ViewData["department"] = await UnitOfWork.DepartmentRepository.GetAll();
            if (id == null)
                return NotFound();
            var student = await UnitOfWork.StudentRepository.Get(id);
            var mapstd=mapper.Map<Student,StudentViewModel>(student);
            return View(ViewName,mapstd);
        }
        [Authorize(Permissions.Students.Edit)]
        public async Task<IActionResult> Edit(int?id)
        {
            ViewData["department"] = await UnitOfWork.DepartmentRepository.GetAll();
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Permissions.Students.Edit)]
        public async Task<IActionResult> Edit([FromRoute] int? id, StudentViewModel student)
        {
            ViewData["department"] = await UnitOfWork.DepartmentRepository.GetAll();
            if (id != student.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                var editstudent=mapper.Map<StudentViewModel,Student>(student);
                await UnitOfWork.StudentRepository.Update(editstudent);
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
            Console.WriteLine("SaveEditStudent action isnot executing...");
            return View("Edit", student);
        }
        [Authorize(Permissions.Students.Delete)]
        public async Task<IActionResult> Delete(int? id)
        {
            ViewData["department"] = await UnitOfWork.DepartmentRepository.GetAll();
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Permissions.Students.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int? id, StudentViewModel student)
        {
            ViewData["department"] = await UnitOfWork.DepartmentRepository.GetAll();
            if (id != student.Id)
                return BadRequest();
            
            try
            {
                var delStudent = mapper.Map<StudentViewModel, Student>(student);
                await UnitOfWork.StudentRepository.Delete(delStudent);
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
