using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using static EmployeeApp.Pages.IndexModel;

namespace EmployeeApp.Pages.Actions
{
    public class UpdateEmployeeModel : PageModel
    {
        [BindProperty]
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

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                employeeInfo.UpdateEmployee();

                TempData["SuccessMessage"] = "Employee Updated Successfully";
                return Redirect("/");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return Page();
            }
        }
    }
}

