using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace EmployeeApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<EmployeeInfo> EmployeeList { get; set; } = new List<EmployeeInfo>();

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Employee;Integrated Security=True;Encrypt=False";
            EmployeeList = new List<EmployeeInfo>();

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
                            EmployeeList.Add(new EmployeeInfo
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                FirstName = reader.GetString(reader.GetOrdinal("firstname")),
                                LastName = reader.GetString(reader.GetOrdinal("lastname")),
                                Position = reader.GetString(reader.GetOrdinal("position")),
                                Department = reader.GetString(reader.GetOrdinal("department"))
                            });
                        }
                    }
                }
            }
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
            public DateTime CreatedAt { get; set; }
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
        }
    }
}
