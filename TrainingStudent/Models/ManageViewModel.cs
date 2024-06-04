using System.Security.Claims;

namespace TrainingStudent.Models
{
    public class ManageViewModel
    {
        public List<string> AccessibleActions { get; set; }
        public string UserId { get; set; }
        public List<Claim> UserClaims { get; set; }
        public List<string> AllPermissions { get; set; }
    }
}
