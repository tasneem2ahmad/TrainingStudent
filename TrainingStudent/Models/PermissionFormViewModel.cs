namespace TrainingStudent.Models
{
    public class PermissionFormViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<CheckboxViewModel> RoleClaim {  get; set; }
        
    }
}
