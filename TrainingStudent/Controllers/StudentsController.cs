using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using Training.BLL.Interfaces;
using Training.DAL.Context;
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
        private readonly SchoolingContext context;

        public StudentsController(IUnitOfWork UnitOfWork, IMapper mapper,SchoolingContext context)
        {

            this.UnitOfWork = UnitOfWork;
            this.mapper = mapper;
            this.context = context;
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
        public async Task<IActionResult> ResultReport(int id)
        {
            if (id == 0)
                return NotFound();
            var studentWithCourse = await context.Students.Where(c=>c.Id==id)
                .Join(context.CourseStudents, s => s.Id, cs => cs.StudentID, (s, cs) => new { Student = s, CourseStudent = cs })
                .Join(context.Courses, sc => sc.CourseStudent.CourseID, c => c.Id, (sc, c) => new StudentWithCourseViewModel
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
                }).ToListAsync();

            var studentData = new List<CourseStudentViewModel>();

            var courseStudents = await UnitOfWork.CourseStudentRepository.GetAll(); // Materialize the query
            foreach(var item in courseStudents)
            {
                if(item.StudentID== id)
                {
                    var CourseofStudentId=item.Id;
                   var addeditem= await UnitOfWork.CourseStudentRepository.Get(CourseofStudentId);
                    var mapaddeditem=mapper.Map<CourseStudent,CourseStudentViewModel>(addeditem);
                    studentData.Add(mapaddeditem);
                }

            }
            if (studentData.Count == 0)
            {
                return NotFound();
            }

            return View(studentWithCourse);
        }
    }





}

