using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static EmployeeApp.Pages.IndexModel;
using System.Data.SqlClient;

namespace EmployeeApp.Pages.Actions
{
    public class ViewEmployeeModel : PageModel
    {
        public EmployeeInfo employeeInfo { get; set; } = new EmployeeInfo();
        public void OnGet()
        {
            string id = Request.Query["id"];

            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Employee;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = @"
                    SELECT e.id, e.firstname, e.lastname, e.email, ed.birthday, ed.phone, ed.address, ed.position, ed.department
                    FROM Employee e
                    INNER JOIN Employee_Details ed ON e.id = ed.id
                    WHERE e.id = @id;
                    ";

                    using (SqlCommand cmd = new SqlCommand(selectQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                employeeInfo = new EmployeeInfo
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    FirstName = reader.GetString(reader.GetOrdinal("firstname")),
                                    LastName = reader.GetString(reader.GetOrdinal("lastname")),
                                    Email = reader.GetString(reader.GetOrdinal("email")),
                                    Birthday = reader.GetDateTime(reader.GetOrdinal("birthday")),
                                    Phone = reader.GetString(reader.GetOrdinal("phone")),
                                    Address = reader.GetString(reader.GetOrdinal("address")),
                                    Position = reader.GetString(reader.GetOrdinal("position")),
                                    Department = reader.GetString(reader.GetOrdinal("department"))
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
        }
    }
}
