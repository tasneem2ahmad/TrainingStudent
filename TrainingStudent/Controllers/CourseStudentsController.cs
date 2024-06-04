using Microsoft.AspNetCore.Mvc;
using Training.DAL.Context;
using AutoMapper;
using Training.BLL.Repositories;
using Training.BLL.Interfaces;
using Training.DAL.Entities;
using TrainingStudent.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using TrainingStudent.Helpers;
using TrainingStudent.Filters;
namespace TrainingStudent.Controllers
{
    [Authorize]
    public class CourseStudentsController : Controller
    {
        public SchoolingContext Context { get; }
        public IMapper Mapper { get; }
        public IUnitOfWork UnitOfWork { get; }

        public CourseStudentsController(SchoolingContext context, IMapper mapper,IUnitOfWork unitOfWork )
        {
            
            Mapper = mapper;
            UnitOfWork = unitOfWork;
            Context = context;
        }
        [Authorize(Permissions.CourseStudents.View)]
        public async Task< IActionResult> Index(string SearchValue)
        {
            ViewData["department"] = await UnitOfWork.DepartmentRepository.GetAll();
            if (string.IsNullOrEmpty(SearchValue))
            {
                var studentWithCourse = await Context.Students
                .Join(Context.CourseStudents, s => s.Id, cs => cs.StudentID, (s, cs) => new { Student = s, CourseStudent = cs })
                .Join(Context.Courses, sc => sc.CourseStudent.CourseID, c => c.Id, (sc, c) => new StudentWithCourseViewModel
                {
                    ID = sc.CourseStudent.Id,
                    StudentId = sc.Student.Id,
                    CourseId = sc.CourseStudent.CourseID,
                    StudentName = sc.Student.Name,
                    StudentEmail = sc.Student.Email,
                    PhoneNumber=sc.Student.PhoneNumber,
                    YearOfSchool=sc.Student.YearofSchool,
                    SchoolGrade=sc.Student.SchoolGrade,
                    StudentDepartment = sc.Student.Department.Name,
                    CourseName = c.Name,
                    CourseDuration = c.Duration,
                    CourseDepartment = c.Department.Name,
                    MidDegree = sc.CourseStudent.MidDegree,
                    PracticalDegree = sc.CourseStudent.PracticalDegree,
                    FinalDegree = sc.CourseStudent.FinalDegree,
                    MaxDegree = sc.CourseStudent.Max,
                }).ToListAsync();

                return View(studentWithCourse);
            }
            else
            {
                var searchStudent = Mapper.Map<IEnumerable<CourseStudent>, IEnumerable<CourseStudentViewModel>>(await UnitOfWork.CourseStudentRepository.SearchCourseStudent(SearchValue));
                return View(searchStudent);
            }
        }

        [Authorize(Permissions.CourseStudents.Details)]

        public async Task<IActionResult>Details(int?id,string ViewName = "Details")
        {
            if(id==null)
                return NotFound();
            ViewData["Course"] = await UnitOfWork.CourseRepository.GetAll();
            ViewData["Student"] = await UnitOfWork.StudentRepository.GetAll();
            var filterStudentWithCourse = await Context.Students
                .Join(Context.CourseStudents, s => s.Id, cs => cs.StudentID, (s, cs) => new { Student = s, CourseStudent = cs })
                .Join(Context.Courses, sc => sc.CourseStudent.CourseID, c => c.Id, (sc, c) => new StudentWithCourseViewModel
                {
                    ID = sc.CourseStudent.Id,
                    StudentId = sc.Student.Id,
                    CourseId = sc.CourseStudent.CourseID,
                    StudentName = sc.Student.Name,
                    StudentEmail = sc.Student.Email,
                    PhoneNumber = sc.Student.PhoneNumber,
                    YearOfSchool = sc.Student.YearofSchool,
                    SchoolGrade = sc.Student.SchoolGrade,
                    StudentDepartment = sc.Student.Department.Name,
                    CourseName = c.Name,
                    CourseDuration = c.Duration,
                    CourseDepartment = c.Department.Name,
                    MidDegree = sc.CourseStudent.MidDegree,
                    PracticalDegree = sc.CourseStudent.PracticalDegree,
                    FinalDegree = sc.CourseStudent.FinalDegree,
                    MaxDegree = sc.CourseStudent.Max,
                }).FirstOrDefaultAsync();
           
            if(filterStudentWithCourse==null)
                return NotFound();
            return View(ViewName, filterStudentWithCourse);
        }
        [Authorize(Permissions.CourseStudents.Create)]
        public async Task<IActionResult> Create()
        {
            ViewData["Course"] = await UnitOfWork.CourseRepository.GetAll();
            ViewData["Student"] = await UnitOfWork.StudentRepository.GetAll();
            
            return View();
        }
        [HttpPost]
        [Authorize(Permissions.CourseStudents.Create)]
        public async Task<IActionResult> Create(CourseStudentViewModel model)
        {
            if (model == null)
                return BadRequest();
            ViewData["Course"] = await UnitOfWork.CourseRepository.GetAll();
            ViewData["Student"] = await UnitOfWork.StudentRepository.GetAll();
            if (ModelState.IsValid)
            {
                var mapCourseStudent=Mapper.Map<CourseStudentViewModel,CourseStudent> (model);
                await UnitOfWork.CourseStudentRepository.Add(mapCourseStudent);
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
            Console.WriteLine("SaveCreateNewCourseStudent action isnot executing...");
            return View("Create", model);
        }
        [Authorize(Permissions.CourseStudents.Edit)]
        public async Task<IActionResult>Edit(int? id)
        {
            ViewData["Course"] = await UnitOfWork.CourseRepository.GetAll();
            ViewData["Student"] = await UnitOfWork.StudentRepository.GetAll();
            return await Details (id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Permissions.CourseStudents.Edit)]
        public async Task<IActionResult> Edit([FromRoute]int? id,CourseStudentViewModel model)
        {
            if (id != model.Id)
                return BadRequest();
            ViewData["Course"] = await UnitOfWork.CourseRepository.GetAll();
            ViewData["Student"] = await UnitOfWork.StudentRepository.GetAll();
            if (ModelState.IsValid)
            {
                var mapmodel=Mapper.Map<CourseStudentViewModel,CourseStudent>(model);
                await UnitOfWork.CourseStudentRepository.Update(mapmodel);
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
            Console.WriteLine("SaveUpdateNewCourseStudent action isnot executing...");
            return View("Edit", model);
        }
        [Authorize(Permissions.CourseStudents.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            ViewData["Course"] = await UnitOfWork.CourseRepository.GetAll();
            ViewData["Student"] = await UnitOfWork.StudentRepository.GetAll();
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Permissions.CourseStudents.Delete)]
        public async Task<IActionResult> Delete([FromRoute]int id,CourseStudentViewModel model)
        {
            if (id != model.Id)
                return BadRequest();
            ViewData["Course"] = await UnitOfWork.CourseRepository.GetAll();
            ViewData["Student"] = await UnitOfWork.StudentRepository.GetAll();
            if (ModelState.IsValid)
            {
                var mapmodel = Mapper.Map<CourseStudentViewModel, CourseStudent>(model);
                await UnitOfWork.CourseStudentRepository.Delete(mapmodel);
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
            Console.WriteLine("SaveDeleteNewCourseStudent action isnot executing...");
            return View("Delete", model);
        }
    }
}
