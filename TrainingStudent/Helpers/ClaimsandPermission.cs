using Training.DAL.Context;

namespace TrainingStudent.Helpers
{
    public class ClaimsandPermission
    {
        public TableService TableService { get; }

        public ClaimsandPermission(TableService tableService)
        {
            TableService = tableService;
        }


        public static List<string>GeneratePermissionList(string module)
        {
            return new List<string>() {
                $"Permission.{module}.Index",
                $"Permission.{module}.Details",
                $"Permission.{module}.Edit",
                $"Permission.{module}.Create",
                $"Permission.{module}.Delete",
            };
        }
        public static List<string> GetAllPermission(TableService tableService)
        {
            var allpermission=new List<string>();
            var modules=tableService.GetTableNames();
            foreach (var module in modules)
            {
                allpermission.AddRange(GeneratePermissionList(module.ToString()));
                
            }
            return allpermission;
        }
    }
}
