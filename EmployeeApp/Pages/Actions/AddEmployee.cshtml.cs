using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static EmployeeApp.Pages.IndexModel;
using System.Data.SqlClient;

namespace EmployeeApp.Pages.Actions
{
    public class AddEmployeeModel : PageModel
    {
        public void OnGet()
        {
        }
        [BindProperty]
        public EmployeeInfo Info { get; set; } = new EmployeeInfo();

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                Info.SaveEmployee();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return Page();
            }

            TempData["SuccessMessage"] = "Employee Added Successfully";
            return Redirect("/");
        }
    }

}
