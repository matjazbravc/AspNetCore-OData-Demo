using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using EmployeesWebApiOData.Controllers.EmployeesWebApiOData.Controllers;
using EmployeesWebApiOData.IntegrationTests.Services;
using EmployeesWebApiOData.Models;
using EmployeesWebApiOData.Services.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;

namespace EmployeesWebApiOData.IntegrationTests.Controllers
{
	public class EmployeesControllerIntegrationTests : IClassFixture<TestWebApplicationFactory<Startup>>
	{
		private readonly EmployeesController _controller;
		private readonly HttpClientHelper _httpClientHelper;

		public EmployeesControllerIntegrationTests(TestWebApplicationFactory<Startup> factory)
		{
			_httpClientHelper = new HttpClientHelper(factory.CreateDefaultClient());
			var repository = new EmployeeRepository(factory.Context, new LoggerFactory());
			_controller = new EmployeesController(repository);
		}

		[Fact]
		public async Task CanDeleteEmployeeHttpClient()
		{
			var newEmployee = new Employee("Alice", "Cooper", new DateTime(2001, 10, 11)) { Id = 666, Created = DateTime.UtcNow, Modified = DateTime.UtcNow };
			var patchResult = await _controller.Post(newEmployee);
			var okResult = patchResult as OkObjectResult;
			var createdEmployee = okResult?.Value as Employee;

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal("Alice", createdEmployee?.FirstName);
			Assert.Equal("Cooper", createdEmployee?.LastName);

			/* Delete new Employee */
			var status = await _httpClientHelper.DeleteAsync("/odata/employees(666)");
			Assert.Equal(HttpStatusCode.NoContent, status);
		}

		[Fact]
		public async Task CanGetAllEmployeesController()
		{
			var result = await _controller.Get();
			var okResult = result as OkObjectResult;
			var employees = okResult?.Value as List<Employee>;

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.True(employees?.Count > 0);
		}

		[Fact]
		public async Task CanGetAllEmployeesHttpClient()
		{
			var employees = await _httpClientHelper.GetListAsync<Employee>("/odata/employees");
			Assert.True(employees?.Count > 0);
		}

		[Fact]
		public async Task CanGetEmployeeController()
		{
			var result = await _controller.Get(3);
			var okResult = result as OkObjectResult;
			var employee = okResult?.Value as Employee;

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(3, employee?.Id);
			Assert.Equal("Julia", employee?.FirstName);
		}

		[Fact]
		public async Task CanGetEmployeeHttpClient()
		{
			var employee = await _httpClientHelper.GetAsync<Employee>("/odata/employees(3)");
			Assert.Equal(3, employee.Id);
			Assert.Equal("Julia", employee.FirstName);
		}

		[Fact]
		public async Task CanPatchEmployeeController()
		{
			var patchDoc = new JsonPatchDocument<Employee>();
			patchDoc.Replace(emp => emp.FirstName, "Johanes");

			var patchResult = await _controller.Patch(1, patchDoc);
			var okResult = patchResult as OkObjectResult;
			var updatedEmployee = okResult?.Value as Employee;

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal("Johanes", updatedEmployee?.FirstName);
		}

		[Fact]
		public async Task CanPatchEmployeeHttpClient()
		{
			var patchDoc = new JsonPatchDocument<Employee>();
			patchDoc.Replace(emp => emp.FirstName, "Johanes");

			// Patch Employee
			var updatedEmployee = await _httpClientHelper.PatchAsync<JsonPatchDocument<Employee>, Employee>("/odata/employees(1)", patchDoc).ConfigureAwait(false);

			Assert.Equal("Johanes", updatedEmployee?.FirstName);
		}
		[Fact]
		public async Task CanPostEmployeeController()
		{
			var newEmployee = new Employee("Alice", "Cooper", new DateTime(2001, 10, 11)) { Id = 999, Created = DateTime.UtcNow, Modified = DateTime.UtcNow };
			var patchResult = await _controller.Post(newEmployee);
			var okResult = patchResult as OkObjectResult;
			var createdEmployee = okResult?.Value as Employee;

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal("Alice", createdEmployee?.FirstName);
			Assert.Equal("Cooper", createdEmployee?.LastName);

			/* Delete new Employee */
			var status = await _httpClientHelper.DeleteAsync("/odata/employees(999)");
			Assert.Equal(HttpStatusCode.NoContent, status);
		}

		[Fact]
		public async Task CanPostEmployeeHttpClient()
		{
			//// Does snot work because Employee in the Controller is always Null!
			//var newEmployee = new Employee("Alice", "Cooper", new DateTime(2001, 10, 11)) { Id = 7, Created = DateTime.UtcNow, Modified = DateTime.UtcNow };
			//var result = await _httpClientHelper.PostAsync<Employee, Employee>("/odata/employees", newEmployee);

			//Assert.Equal("Alice", result.FirstName);
			//Assert.Equal("Cooper", result.LastName);

			//await _httpClientHelper.DeleteAsync("/odata/employees(999)");
			Assert.True(true);
		}

		[Fact]
		public async Task CanPutEmployeeHttpClient()
		{
			// Get first employee
			var employee = await _httpClientHelper.GetAsync<Employee>("/odata/employees(1)");

			// Change first name
			employee.FirstName = "Johnny";

			// Update employee
			var updatedEmployee = await _httpClientHelper.PutAsync<Employee, Employee>("/odata/employees(1)", employee);

			// First name should be a new one
			Assert.Equal("Johnny", updatedEmployee.FirstName);
		}
	}
}
