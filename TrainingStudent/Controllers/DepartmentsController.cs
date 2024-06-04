using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Training.BLL.Interfaces;
using Training.DAL.Context;
using Training.DAL.Entities;
using TrainingStudent.Filters;
using TrainingStudent.Helpers;


namespace TrainingStudent.Controllers
{
    [Authorize]

    public class DepartmentsController : Controller
    {
        private readonly SchoolingContext context;
        

        public IUnitOfWork UnitOfWork { get; }

        public DepartmentsController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        [Authorize(Permissions.Departments.View)]
        public async Task<IActionResult> Index()
        {
            
            return View(await UnitOfWork.DepartmentRepository.GetAll());
        }
        [Authorize(Permissions.Departments.Create)]

        public async Task<IActionResult> CreateNewDepartment()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Permissions.Departments.Create)]

        public async Task<IActionResult> CreateNewDepartment(Department newDepart)
        {
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
                Console.WriteLine("SaveCreateNewDepartment action isnot executing...");

                return View("CreateNewDepartment", newDepart);
            }
            var depart = new Department()
            {
                Name = newDepart.Name
            };
            //context.Departments.Add(depart);
            //context.SaveChanges();
            await UnitOfWork.DepartmentRepository.Add(depart);
            return RedirectToAction("Index");
                
        }
        [Authorize(Permissions.Departments.Details)]
        public async Task<IActionResult> Details(int? id,string ViewName="Details")
        {
            if (id == null)
                return NotFound();
            var department = await UnitOfWork.DepartmentRepository.Get(id);
            return View(ViewName,department);
        }
        [Authorize(Permissions.Departments.Edit)]

        public async Task<IActionResult> Edit(int id)
            
        {
            return await Details(id,"Edit");
 
        }
        [HttpPost]//[HttpPut] in API
        [ValidateAntiForgeryToken] //no one can call edit unless using its form 
        [Authorize(Permissions.Departments.Edit)]

        public async Task<IActionResult> Edit([FromRoute]int? id,Department department)
        {
            if (id != department.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    await UnitOfWork.DepartmentRepository.Update(department);
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                    return View(department);
                }
            }
            return View(department);
        }
        [Authorize(Permissions.Departments.Delete)]

        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost] //[HttpDelete] in API
        [ValidateAntiForgeryToken]//no one can call edit unless using its form 
        [Authorize(Permissions.Departments.Delete)]

        public async Task<IActionResult> Delete([FromRoute]int?id,Department department)
        {
            if (id != department.Id)
                return BadRequest(); //status code 400
            var isUsedInOtherTable = false;
            var tableName = string.Empty;

            // Example check in 'Employees' table
            if (context.Courses.Any(e => e.DepartmentID == id))
            {
                isUsedInOtherTable = true;
                tableName = "Courses";
            }else if (context.Teachers.Any(e => e.DepartmentId == id))
            {
                isUsedInOtherTable = true;
                tableName = "Teachers";
            }
            // Add similar checks for other tables where DepartmentId might be used

            if (isUsedInOtherTable)
            {
                TempData["ErrorMessage"] = $"Cannot delete record because it's being used in the {tableName} table.";
                // Return a message that the department is being used in another table
                return View(department);
            }
            else
            {
                try
                {
                    await UnitOfWork.DepartmentRepository.Delete(department);
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View(department);
                }
            }
            

        }


    }
}
