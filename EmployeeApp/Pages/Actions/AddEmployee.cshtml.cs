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
                Info.CreatedAt = DateTime.Now;

                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Employee;Integrated Security=True;Encrypt=False";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string insertEmployeeQuery = @"
                INSERT INTO Employee (firstname, lastname, email, created_at)
                OUTPUT INSERTED.id
                VALUES (@FirstName, @LastName, @Email, @CreatedAt);
                ";

                    using (SqlCommand cmd = new SqlCommand(insertEmployeeQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", Info.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", Info.LastName);
                        cmd.Parameters.AddWithValue("@Email", Info.Email);
                        cmd.Parameters.AddWithValue("@CreatedAt", Info.CreatedAt);

                        int insertedId = (int)cmd.ExecuteScalar();
                        string insertDetailsQuery = @"
                    INSERT INTO Employee_Details (id, birthday, phone, address, position, department)
                    VALUES (@Id, @Birthday, @Phone, @Address, @Position, @Department);
                    ";

                        using (SqlCommand cmdDetails = new SqlCommand(insertDetailsQuery, connection))
                        {
                            cmdDetails.Parameters.AddWithValue("@Id", insertedId);
                            cmdDetails.Parameters.AddWithValue("@Birthday", Info.Birthday);
                            cmdDetails.Parameters.AddWithValue("@Phone", Info.Phone);
                            cmdDetails.Parameters.AddWithValue("@Address", Info.Address);
                            cmdDetails.Parameters.AddWithValue("@Position", Info.Position);
                            cmdDetails.Parameters.AddWithValue("@Department", Info.Department);

                            cmdDetails.ExecuteNonQuery();
                        }
                    }
                }
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
