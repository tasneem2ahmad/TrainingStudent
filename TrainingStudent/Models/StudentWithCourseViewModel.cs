namespace TrainingStudent.Models
{
    public class StudentWithCourseViewModel
    {
        public int ID { get; set; }
        public string PhoneNumber { get; set; }
        public int YearOfSchool { get; set; }
        public string SchoolGrade {  get; set; }

        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public string StudentName {  get; set; }
        public string StudentEmail { get; set; }
        public string StudentDepartment { get; set; }
        public string CourseName {  get; set; }
        public string CourseDuration { get; set; }
        public string CourseDepartment {  get; set; }
        public decimal MidDegree { get; set; }
        public decimal PracticalDegree {  get; set; }
        public decimal FinalDegree {  get; set; }
        public decimal MaxDegree {  get; set; }
        public bool IsDeleted { get; set; }

    }
}
