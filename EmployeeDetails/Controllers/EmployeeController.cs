using EmployeeDetails.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace EmployeeDetails.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(IEmployeeRepository employeeRepository, ILogger<EmployeesController> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

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

        [HttpDelete("deleteEmployee/{id}")]
        public async Task<IActionResult> deleteEmployee(int id)
        {
            await _employeeRepository.deleteEmployeeAsync(id);
            _logger.LogInformation("employee data deleted successfully.");
            return NoContent();
        }
        [HttpGet("deleteEmployee1/{id}")]
        public async Task<IActionResult> deleteEmployeeURL(int id)
        {
            await _employeeRepository.deleteEmployeeAsync(id);
            _logger.LogInformation("employee data deleted successfully.");
            return NoContent();
        }
    }
}
