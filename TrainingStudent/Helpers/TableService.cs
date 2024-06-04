using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.ComponentModel;
using System.Configuration;
using Training.DAL.Context;

namespace TrainingStudent.Helpers
{
    public class TableService
    {
        private readonly SchoolingContext _context;

        public TableService(SchoolingContext context)
        {
            _context = context;
        }

        public List<string> GetTableNames()
        {
            var tableNames = new List<string>();

            var entityTypes = _context.Model.GetEntityTypes();

            var identityTableNames = new List<string>
            {
                "AspNetUsers",
                "AspNetRoles",
                "AspNetUserRoles",
                "AspNetUserClaims",
                "AspNetUserLogins",
                "AspNetRoleClaims",
                "AspNetUserTokens"
            };
            string exceptedtable = "CourseStudent";
            string wantedtable = "CourseStudents";
            string exceptteacheredtable = "StudentTeacher";
            string wantedteachertable = "StudentTeachers";
            foreach (var entityType in entityTypes)
            {
                var tableNameAnnotation = entityType.GetAnnotation("Relational:TableName");

                var tableName = tableNameAnnotation?.Value?.ToString();
                if (!string.IsNullOrEmpty(tableName) && !identityTableNames.Contains(tableName)&& !tableName.Contains(exceptedtable) && !tableName.Contains(exceptteacheredtable))
                {
                    tableNames.Add(tableName);
                }
            }
            tableNames.Add(wantedtable);
            tableNames.Add(wantedteachertable);
            return tableNames;
        }
    }
   
    
}
