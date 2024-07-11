using EmployeeDetails.Controllers;
using EmployeeDetails.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace TestEmployee
{
    public class EmployeesControllerTests
    {       
            private readonly Mock<IEmployeeRepository> _mockRepo;
            private readonly Mock<ILogger<EmployeesController>> _mockLogger;
            private readonly EmployeesController _controller;

            public EmployeesControllerTests()
            {
                _mockRepo = new Mock<IEmployeeRepository>();
                _mockLogger = new Mock<ILogger<EmployeesController>>();
                _controller = new EmployeesController(_mockRepo.Object, _mockLogger.Object);
            }

            [Fact]
            public async Task GetAllEmployees_ReturnsOkResult_WithListOfEmployees()
            {
                // Arrange
                var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "Mark", Department = "IT", Email = "Mark@hotmail.com"},
                new Employee { Id = 2, Name = "Param",Department = "IT", Email = "param@hotmail.com"},
                new Employee {Id = 3, Name = "Tom", Department = "HR", Email = "Tom@hotmail.com"},
            };
                _mockRepo.Setup(repo => repo.getAllEmployeesAsync()).ReturnsAsync(employees);

                // Act
                var result = await _controller.getAllEmployees();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var returnEmployees = Assert.IsType<List<Employee>>(okResult.Value);
                Assert.Equal(3, returnEmployees.Count);
            }

        [Fact]
        public async Task GetAllEmployees_ReturnsOkResult_WithEmptyList_WhenRepositoryThrowsException()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.getAllEmployeesAsync()).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.getAllEmployees();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var employees = Assert.IsType<List<Employee>>(okResult.Value);
            Assert.Empty(employees);
        }


        [Fact]
            public async Task GetEmployeeById_ReturnsOkResult_WithEmployee()
            {
                // Arrange
                var employee = new Employee { Id = 1, Name = "Mark", Department = "IT", Email = "Mark@hotmail.com" };
                _mockRepo.Setup(repo => repo.getEmployeeByIdAsync(1)).ReturnsAsync(employee);

                // Act
                var result = await _controller.getEmployeeById(1);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var returnEmployee = Assert.IsType<Employee>(okResult.Value);
                Assert.Equal(employee.Id, returnEmployee.Id);
            }       

        [Fact]
            public async Task GetEmployeeById_ReturnsNotFoundResult_WhenEmployeeNotFound()
            {
                // Arrange
                _mockRepo.Setup(repo => repo.getEmployeeByIdAsync(1)).ReturnsAsync((Employee)null);

                // Act
                var result = await _controller.getEmployeeById(1);

                // Assert
                Assert.IsType<NotFoundResult>(result.Result);
            }

            [Fact]
            public async Task AddEmployee_ReturnsCreatedAtActionResult_WithEmployee()
            {
                // Arrange
                var employee = new Employee { Id = 4, Name = "Mark", Department = "IT", Email = "Mark@hotmail.com" };
                _mockRepo.Setup(repo => repo.addEmployeeAsync(employee)).ReturnsAsync(employee);

                // Act
                var result = await _controller.addEmployee(employee);

                // Assert
                var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
                var returnEmployee = Assert.IsType<Employee>(createdAtActionResult.Value);
                Assert.Equal(employee.Id, returnEmployee.Id);
            }
        [Fact]
        public async Task AddEmployee_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.addEmployee(new Employee());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var modelState = Assert.IsType<SerializableError>(badRequestResult.Value);
            Assert.True(modelState.ContainsKey("Name"));
        }


        [Fact]
            public async Task UpdateEmployee_ReturnsNoContentResult_WhenEmployeeUpdated()
            {
                // Arrange
                var employee = new Employee { Id = 1, Name = "Mark1", Department = "IT", Email = "Mark@hotmail.com" };
                _mockRepo.Setup(repo => repo.updateEmployeeAsync(employee)).ReturnsAsync(employee);

                // Act
                var result = await _controller.updateEmployee(1,employee);

                // Assert
                Assert.IsType<NoContentResult>(result);
            }

        [Fact]
        public async Task UpdateEmployee_ReturnsBadRequest_WhenIdMismatch()
        {
            // Arrange
            var employee = new Employee { Id = 1, Name = "Mark1", Department = "IT", Email = "Mark@hotmail.com" };

            // Act
            var result = await _controller.updateEmployee(1,employee);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task UpdateEmployee_ReturnsNotFound_WhenEmployeeDoesNotExist()
        {
            // Arrange
            var employee = new Employee { Id = 1, Name = "Mark1", Department = "IT", Email = "Mark@hotmail.com" };
            _mockRepo.Setup(repo => repo.updateEmployeeAsync(employee)).ReturnsAsync((Employee)null);

            // Act
            var result = await _controller.updateEmployee(1,employee);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
            public async Task DeleteEmployee_ReturnsNoContentResult()
            {
                // Arrange
                _mockRepo.Setup(repo => repo.deleteEmployeeAsync(1)).Returns(Task.CompletedTask);

                // Act
                var result = await _controller.deleteEmployee(1);

                // Assert
                Assert.IsType<NoContentResult>(result);
            }

        [Fact]
        public async Task DeleteEmployee_ReturnsNoContent_WhenEmployeeDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.deleteEmployeeAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.deleteEmployee(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }


    }
}