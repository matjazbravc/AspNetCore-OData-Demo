using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeesWebApiOData.Models;
using EmployeesWebApiOData.Services.Repositories;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeesWebApiOData.Controllers
{
	namespace EmployeesWebApiOData.Controllers
	{
		[Produces("application/json")]
		public class EmployeesController : ODataController
		{
			private readonly IEmployeeRepository _repository;

			public EmployeesController(IEmployeeRepository repository)
			{
				_repository = repository;
			}

			/// <summary>
			/// Use the DELETE http verb
			/// Request for odata/employees(5)
			/// </summary>
			/// <param name="key"></param>
			/// <returns></returns>
			[ProducesResponseType(204)]
			[ProducesResponseType(404)]
			public async Task<IActionResult> Delete([FromODataUri] int key)
			{
				var entity = await _repository.FindOneAsync(employee => employee.Id == key);
				if (entity == null)
				{
					return NotFound();
				}
				await _repository.DeleteAsync(entity);
				return NoContent();
			}

			/// <summary>
			/// Gets all Employees
			/// Use the GET http verb
			/// Request for odata/employees
			/// </summary>
			/// <returns>List of Employees</returns>
			[EnableQuery(PageSize = 10, MaxExpansionDepth = 5)]
			[ProducesResponseType(200, Type = typeof(IEnumerable<Employee>))]
			[ProducesResponseType(404)]
			public async Task<IActionResult> Get()
			{
				var entity = await _repository.FindAsync();
				if (entity == null)
				{
					return NotFound();
				}
				return Ok(entity);
			}

			/// <summary>
			/// Gets single Employee
			/// Use the GET http verb
			/// Request for odata/employees(3)
			/// </summary>
			/// <param name="key"></param>
			/// <returns>Single Employee</returns>
			[HttpGet]
			[ProducesResponseType(200, Type = typeof(Employee))]
			[ProducesResponseType(404)]
			public async Task<IActionResult> Get([FromODataUri] int key)
			{
				var entity = await _repository.FindOneAsync(emp => emp.Id== key);
				if (entity == null)
				{
					return NotFound();
				}
				return Ok(entity);
			}

			/// <summary>
			/// This method is like PUT except that in PUT all the properties of the passed-in object are copied
			/// into an exisitng object, where as in PATCH, only properties that have changed are applied to an
			/// exisitng object. Property changes are passed using the Delta class object.
			/// Use the PATCH http verb
 			/// Request for odata/employees(1)
			/// Set Content-Type:Application/Json
			/// Call this using following in request body: { "FirstName":"Suzy" }
			/// </summary>
			/// <param name="key"></param>
			/// <param name="patch"></param>
			/// <returns>HTTP status code OK (200) and the entire object in JSON format in the response body</returns>
			[AcceptVerbs("PATCH", "MERGE")]
			[EnableQuery]
			[ProducesResponseType(200, Type = typeof(Employee))]
			[ProducesResponseType(400)]
			[ProducesResponseType(404)]
			public async Task<IActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<Employee> patch)
			{
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}
				var entity = await _repository.FindOneAsync(emp => emp.Id== key);
				if (entity == null)
				{
					return NotFound();
				}
				patch.ApplyTo(entity, ModelState);
				if (!ModelState.IsValid)
				{
					return new BadRequestObjectResult(ModelState);
				}
				try
				{
					await _repository.UpdateAsync(entity);
				}
				catch (DbUpdateConcurrencyException)
				{
					if (await _repository.FindOneAsync(emp => emp.Id== key) == null)
					{
						return NotFound();
					}
					throw;
				}
				return Ok(await _repository.FindOneAsync(emp => emp.Id == key));
			}

			/// <summary>
			/// Creates a new Employee. 
			/// Use the POST http verb.
			/// Request for odata/employees
			/// Set Content-Type:Application/Json
			/// Call this using following in request body: { "Id": 7, "FirstName": "Johny", "LastName": "Walker", "BirthDate": "2001-05-31", "Age": 33 }
			/// </summary>
			/// <param name="employee">Employee</param>
			/// <returns></returns>
			[HttpPost]
			[ProducesResponseType(200, Type = typeof(Employee))]
			[ProducesResponseType(400)]
			public async Task<IActionResult> Post([FromBody] Employee employee)
			{
				if (employee == null)
				{
					return Unauthorized();
				}
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}
				await _repository.InsertAsync(employee);
				return Ok(await _repository.FindOneAsync(emp => emp.Id == employee.Id));
			}

			/// <summary>
			/// Saves the entire Employee object to the object specified by key (id). Is supposed to overwrite all properties.
			/// Use the PUT http verb
			/// Request for odata/employees(1)
			/// Set Content-Type:Application/Json
			/// Call this using following in request body: { "Id": 1, "FirstName": "John", "LastName": "Whyne", "BirthDate": "1991-08-07", "Age": 27 }
			/// </summary>
			/// <param name="key"></param>
			/// <param name="employee"></param>
			/// <returns></returns>
			[HttpPut]
			[ProducesResponseType(200, Type = typeof(Employee))]
			[ProducesResponseType(400)]
			[ProducesResponseType(401)]
			public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] Employee employee)
			{
				if (employee == null)
				{
					return Unauthorized();
				}
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}
				if (!key.Equals(employee.Id))
				{
					return BadRequest();
				}
				try
				{
					await _repository.UpdateAsync(employee);
				}
				catch (DbUpdateConcurrencyException)
				{
					if (await _repository.FindOneAsync(emp => emp.Id == key) == null)
					{
						return NotFound();
					}
					throw;
				}
				return Ok(await _repository.FindOneAsync(emp => emp.Id == key));
			}
		}
	}
}
