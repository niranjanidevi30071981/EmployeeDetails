namespace EmployeeDetails.Models
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> getAllEmployeesAsync();
        Task<Employee> getEmployeeByIdAsync(int id);
        Task<Employee> addEmployeeAsync(Employee employee);
        Task<Employee> updateEmployeeAsync(Employee employee);
        Task deleteEmployeeAsync(int id);      
    }
}
