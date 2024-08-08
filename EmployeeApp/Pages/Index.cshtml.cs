using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using static EmployeeApp.Pages.IndexModel;

namespace EmployeeApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<Dictionary<string, string>> employeeList;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            EmployeeInfo employees = new EmployeeInfo();
            employeeList = employees.GetAllEmployee();

            Console.WriteLine(employeeList);
        }

        public class EmployeeInfo
        {
            public int Id { get; set; }
            [BindProperty]
            [Required]
            public string FirstName { get; set; }
            [BindProperty]
            [Required]
            public string LastName { get; set; }
            [BindProperty]
            [Required]
            public string Email { get; set; }
            [BindProperty]
            [Required]
            public DateTime Birthday { get; set; }
            [BindProperty]
            [Required]
            public string Phone { get; set; }
            [BindProperty]
            [Required]
            public string Address { get; set; }
            [BindProperty]
            [Required]
            public string Position { get; set; }
            [BindProperty]
            [Required]
            public string Department { get; set; }

            private string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Employee;Integrated Security=True;Encrypt=False";

            public void GetEmployee()
            {
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
                        cmd.Parameters.AddWithValue("@id", Id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id"));
                                FirstName = reader.GetString(reader.GetOrdinal("firstname"));
                                LastName = reader.GetString(reader.GetOrdinal("lastname"));
                                Email = reader.GetString(reader.GetOrdinal("email"));
                                Birthday = reader.GetDateTime(reader.GetOrdinal("birthday"));
                                Phone = reader.GetString(reader.GetOrdinal("phone"));
                                Address = reader.GetString(reader.GetOrdinal("address"));
                                Position = reader.GetString(reader.GetOrdinal("position"));
                                Department = reader.GetString(reader.GetOrdinal("department"));
                            }
                        }
                    }
                }
            }

            public List<Dictionary<string, string>> GetAllEmployee()
            {
                List<Dictionary<string, string>> employeeList = new List<Dictionary<string, string>>();
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Employee;Integrated Security=True;Encrypt=False";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = @"
                    SELECT e.id, e.firstname, e.lastname, ed.position, ed.department
                    FROM Employee e
                    INNER JOIN Employee_Details ed ON e.id = ed.id;
                    ";

                    using (SqlCommand cmd = new SqlCommand(selectQuery, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var employeeDetails = new Dictionary<string, string>
                                {
                                    { "Id", reader.GetInt32(reader.GetOrdinal("id")).ToString() },
                                    { "FirstName", reader.GetString(reader.GetOrdinal("firstname")) },
                                    { "LastName", reader.GetString(reader.GetOrdinal("lastname")) },
                                    { "Position", reader.GetString(reader.GetOrdinal("position")) },
                                    { "Department", reader.GetString(reader.GetOrdinal("department")) }
                                };

                                employeeList.Add(employeeDetails);
                            }
                        }
                    }
                }
                return employeeList;
            }

            public void SaveEmployee()
            {
                DateTime CreatedAt = DateTime.Now;
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
                        cmd.Parameters.AddWithValue("@FirstName", FirstName);
                        cmd.Parameters.AddWithValue("@LastName", LastName);
                        cmd.Parameters.AddWithValue("@Email", Email);
                        cmd.Parameters.AddWithValue("@CreatedAt", CreatedAt);

                        int insertedId = (int)cmd.ExecuteScalar();
                        string insertDetailsQuery = @"
                        INSERT INTO Employee_Details (id, birthday, phone, address, position, department)
                        VALUES (@Id, @Birthday, @Phone, @Address, @Position, @Department);
                        ";

                        using (SqlCommand cmdDetails = new SqlCommand(insertDetailsQuery, connection))
                        {
                            cmdDetails.Parameters.AddWithValue("@Id", insertedId);
                            cmdDetails.Parameters.AddWithValue("@Birthday", Birthday);
                            cmdDetails.Parameters.AddWithValue("@Phone", Phone);
                            cmdDetails.Parameters.AddWithValue("@Address", Address);
                            cmdDetails.Parameters.AddWithValue("@Position", Position);
                            cmdDetails.Parameters.AddWithValue("@Department", Department);

                            cmdDetails.ExecuteNonQuery();
                        }
                    }
                }
            }

            public void UpdateEmployee()
            {
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
                        cmd.Parameters.AddWithValue("@Id", Id);
                        cmd.Parameters.AddWithValue("@FirstName", FirstName);
                        cmd.Parameters.AddWithValue("@LastName", LastName);
                        cmd.Parameters.AddWithValue("@Email", Email);

                        cmd.ExecuteNonQuery();
                    }

                    string updateDetailsQuery = @"
                    UPDATE Employee_Details
                    SET birthday = @Birthday, phone = @Phone, address = @Address, position = @Position, department = @Department
                    WHERE id = @Id;
                    ";

                    using (SqlCommand cmdDetails = new SqlCommand(updateDetailsQuery, connection))
                    {
                        cmdDetails.Parameters.AddWithValue("@Id", Id);
                        cmdDetails.Parameters.AddWithValue("@Birthday", Birthday);
                        cmdDetails.Parameters.AddWithValue("@Phone", Phone);
                        cmdDetails.Parameters.AddWithValue("@Address", Address);
                        cmdDetails.Parameters.AddWithValue("@Position", Position);
                        cmdDetails.Parameters.AddWithValue("@Department", Department);

                        cmdDetails.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
