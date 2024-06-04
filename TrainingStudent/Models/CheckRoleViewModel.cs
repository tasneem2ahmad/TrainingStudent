using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;

namespace TrainingStudent.Models
{
    public class CheckRoleViewModel
    {
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}
