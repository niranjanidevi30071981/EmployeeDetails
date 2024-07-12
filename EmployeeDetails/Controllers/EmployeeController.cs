using EmployeeDetails.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace EmployeeDetails.Controllers
{


    /// <summary>
    /// Ticket No:<<>>
    /// EmployeeController handles API requests related to Employee operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        //Local object declaration
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeesController> _logger;

        /// <summary>
        /// Ticket No:<<>>
        /// Initializes a new instance of the EmployeesController class objects using constructor.
        /// </summary>
        /// <param name="employeeRepository">The employee repository instance.</param>
        /// <param name="logger">The logger instance.</param>
        public EmployeesController(IEmployeeRepository employeeRepository, ILogger<EmployeesController> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        /// <summary>
        /// Ticket No:<<>>
        /// Gets the list of all employees .
        /// </summary>
        /// <param name="id">The ID of the employee to retrieve.</param>
        /// <returns>The employee with the specified ID.</returns>
        /// <response code="200">Returns the employee.</response>       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> getAllEmployees()
        {
            IEnumerable<Employee> employees=null;
            try
            {
                employees = await _employeeRepository.getAllEmployeesAsync();
                _logger.LogInformation("GET Employee request received.");               
            }
            catch (Exception ex) {
                _logger.LogInformation(" Controller GetAllEmployees Error :" +ex.Message);
            }
            return Ok(employees);
        }


        /// <summary>
        /// Ticket No:<<>>
        /// Gets an employee by their ID.
        /// </summary>
        /// <param name="id">The ID of the employee to retrieve.</param>
        /// <returns>The employee with the specified ID.</returns>
        /// <response code="200">Returns the employee.</response>
        /// <response code="404">If the employee is not found.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> getEmployeeById(int id)
        {
            var employee = await _employeeRepository.getEmployeeByIdAsync(id);
            if (employee == null)
            {
                _logger.LogWarning("employee data not found.");
                return NotFound();
            }
            return Ok(employee);
        }

        /// <summary>
        /// Ticket No:<<>>
        /// Adds a new employee .
        /// </summary>
        /// <param name="employee">The employee data to add.</param>
        /// <returns>Returns the created employee along with its location.</returns>
        /// <response code="201">If the employee is successfully created.</response>
        /// <response code="400">If the submitted employee data is invalid.</response>
        [HttpPost]
        public async Task<ActionResult<Employee>> addEmployee([FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid employee data submitted.");
                return BadRequest(ModelState);
            }

            var createdEmployee = await _employeeRepository.addEmployeeAsync(employee);
            return CreatedAtAction(nameof(getEmployeeById), new { id = createdEmployee.Id }, createdEmployee);

            _logger.LogInformation("Employee created successfully.");
          
        }

        /// <summary>
        /// Ticket No:<<>>
        /// Updates an existing employee.
        /// </summary>
        /// <param name="id">The ID of the employee to update.</param>
        /// <param name="employee">The employee data to update.</param>
        /// <returns>Returns NoContent if the update is successful; otherwise, BadRequest or NotFound.</returns>
        /// <response code="204">If the update is successful.</response>
        /// <response code="400">If the employee ID mismatch occurs.</response>
        /// <response code="404">If the employee data is not found.</response>
        [HttpPut("updateEmployee/{id}")]
        public async Task<IActionResult> updateEmployee(int id,[FromBody] Employee employee)
        {
            if (id != employee.Id)
            {
                _logger.LogWarning("employee Id was invalid.");
                return BadRequest();
            }

            var updatedEmployee = await _employeeRepository.updateEmployeeAsync(employee);
            if (updatedEmployee == null)
            {
                _logger.LogWarning("employee data not found.");
                return NotFound();
            }
            _logger.LogInformation("employee data updated successfully.");
            return NoContent();
        }

        /// <summary>
        /// Ticket No:<<>>
        /// Delete the employee as per give employee Id.
        /// </summary>
        /// <param name="id">The ID of the employee to Delete.</param>
        /// <returns>Returns NoContent if the update is successful;</returns>       
        [HttpDelete("deleteEmployee/{id}")]
        public async Task<IActionResult> deleteEmployee(int id)
        {
            await _employeeRepository.deleteEmployeeAsync(id);
            _logger.LogInformation("employee data deleted successfully.");
            return NoContent();
        }
        
    }
}
