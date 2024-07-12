using System.ComponentModel.DataAnnotations;

namespace EmployeeDetails.Models
{
    /// <summary>
    /// Ticket No:<<>>
    /// Creating the structure of the Employee records.
    /// </summary>
    public class Employee
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email Address is required.")]
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Department is required.")]
        public string? Department { get; set; }        

    }
}
