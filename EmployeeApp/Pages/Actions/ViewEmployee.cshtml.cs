using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static EmployeeApp.Pages.IndexModel;
using System.Data.SqlClient;

namespace EmployeeApp.Pages.Actions
{
    public class ViewEmployeeModel : PageModel
    {
        public EmployeeInfo employeeInfo { get; set; } = new EmployeeInfo();
        public void OnGet(int id)
        {
            try
            {
                employeeInfo.Id = id;
                employeeInfo.GetEmployee();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
        }
    }
}
