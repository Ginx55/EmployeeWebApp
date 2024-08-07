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

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Employee;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string updateEmployeeQuery = @"
                    UPDATE Employee
                    SET firstname = @FirstName, lastname = @LastName, email = @Email
                    WHERE id = @Id;
                    ";

                    using (SqlCommand cmd = new SqlCommand(updateEmployeeQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Id", employeeInfo.Id);
                        cmd.Parameters.AddWithValue("@FirstName", employeeInfo.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", employeeInfo.LastName);
                        cmd.Parameters.AddWithValue("@Email", employeeInfo.Email);

                        cmd.ExecuteNonQuery();
                    }

                    string updateDetailsQuery = @"
                    UPDATE Employee_Details
                    SET birthday = @Birthday, phone = @Phone, address = @Address, position = @Position, department = @Department
                    WHERE id = @Id;
                    ";

                    using (SqlCommand cmdDetails = new SqlCommand(updateDetailsQuery, connection))
                    {
                        cmdDetails.Parameters.AddWithValue("@Id", employeeInfo.Id);
                        cmdDetails.Parameters.AddWithValue("@Birthday", employeeInfo.Birthday);
                        cmdDetails.Parameters.AddWithValue("@Phone", employeeInfo.Phone);
                        cmdDetails.Parameters.AddWithValue("@Address", employeeInfo.Address);
                        cmdDetails.Parameters.AddWithValue("@Position", employeeInfo.Position);
                        cmdDetails.Parameters.AddWithValue("@Department", employeeInfo.Department);

                        cmdDetails.ExecuteNonQuery();
                    }
                }

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

