using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace TrainingStudent.Helpers
{
    public static class Permissions
    {
        public const string Permission = "Permission";
        public static class Teachers
        {
            public const string Details = "Permission.Teachers.Details";
            public const string View = "Permission.Teachers.Index";
            public const string Create = "Permission.Teachers.Create";
            public const string Edit = "Permission.Teachers.Edit";
            public const string Delete = "Permission.Teachers.Delete";
        };
        public static class Students
        {
            public const string Details = "Permission.Students.Details";
            public const string View = "Permission.Students.Index";
            public const string Create = "Permission.Students.Create";
            public const string Edit = "Permission.Students.Edit";
            public const string Delete = "Permission.Students.Delete";
        };
        public static class Courses
        {
            public const string Details = "Permission.Courses.Details";
            public const string View = "Permission.Courses.Index";
            public const string Create = "Permission.Courses.Create";
            public const string Edit = "Permission.Courses.Edit";
            public const string Delete = "Permission.Courses.Delete";
        };
        public static class Departments
        {
            public const string Details = "Permission.Departments.Details";
            public const string View = "Permission.Departments.Index";
            public const string Create = "Permission.Departments.Create";
            public const string Edit = "Permission.Departments.Edit";
            public const string Delete = "Permission.Departments.Delete";
        };
        public static class StudentTeachers
        {
            public const string Details = "Permission.StudentTeachers.Details";
            public const string View = "Permission.StudentTeachers.Index";
            public const string Create = "Permission.StudentTeachers.Create";
            public const string Edit = "Permission.StudentTeachers.Edit";
            public const string Delete = "Permission.StudentTeachers.Delete";
        };
        public static class CourseStudents
        {
            public const string Details = "Permission.CourseStudents.Details";
            public const string View = "Permission.CourseStudents.Index";
            public const string Create = "Permission.CourseStudents.Create";
            public const string Edit = "Permission.CourseStudents.Edit";
            public const string Delete = "Permission.CourseStudents.Delete";
        };
        




    }
    
}
