
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeDetails.Models
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly List<Employee> _employees;
        private readonly ILogger<EmployeeRepository> _logger;

        public EmployeeRepository(ILogger<EmployeeRepository> logger)
        {
            _logger = logger;

        _employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "Mark", Department ="IT", Email = "Mark@hotmail.com"},
                new Employee { Id = 2, Name = "Param",Department ="IT",Email = "param@hotmail.com"},
                new Employee {Id = 3, Name = "Tom", Department ="HR", Email = "Tom@hotmail.com"},
            };
        }

        public Task<IEnumerable<Employee>> getAllEmployeesAsync()
        {
            Task<IEnumerable<Employee>> result = null;
            try
            {
                result= Task.FromResult(_employees.AsEnumerable());
            }
            catch (Exception ex)
            {
                _logger.LogError("GetAllEmployeesAsync error:"+ex.Message);
            }
            return  result;
        }

        public Task<Employee> getEmployeeByIdAsync(int id)
        {
            Task<Employee> result=null;
           
            try
            {
                var employee = _employees.FirstOrDefault(e => e.Id == id);
                 result = Task.FromResult(employee);
            }
            catch (Exception ex) {
                _logger.LogError("GetEmployeeByIdAsync error:" + ex.Message);
            }

            return result;
        }

        public Task<Employee> addEmployeeAsync(Employee employee)
        {
            Task<Employee> result = null;
           
            try
            {
                employee.Id = _employees.Count + 1;
                _employees.Add(employee);
                result= Task.FromResult(employee);

            }
            catch (Exception ex) {
                _logger.LogError("AddEmployeeAsync error:" + ex.Message);
            }
            return result;
        }

        public Task<Employee> updateEmployeeAsync(Employee employee)
        {
            Task<Employee> result = null;

            try
            {
                var existingEmployee = _employees.FirstOrDefault(e => e.Id == employee.Id);
                if (existingEmployee != null)
                {
                    existingEmployee.Name = employee.Name;
                    existingEmployee.Email = employee.Email;
                    existingEmployee.Department = employee.Department;

                }
                result= Task.FromResult(existingEmployee);

            }
            catch(Exception ex) {
                _logger.LogError("UpdateEmployeeAsync error:" + ex.Message);
            }
          
            return result;
        }

        public Task deleteEmployeeAsync(int id)
        {
            Task<Employee> result = null;
           
            try
            {
                var employee = _employees.FirstOrDefault(e => e.Id == id);
                if (employee != null)
                {
                    _employees.Remove(employee);
                }
                return Task.CompletedTask;
            }
            catch(Exception ex)
            {
                _logger.LogError("DeleteEmployeeAsync error:" + ex.Message);
                return null;

            }
          

        }
    }
}
