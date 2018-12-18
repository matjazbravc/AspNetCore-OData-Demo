using System;
using System.Linq;
using EmployeesWebApiOData.DbContexts;
using EmployeesWebApiOData.Models;

namespace EmployeesWebApiOData.Services
{
	public static class SeedData
	{
		public static void Initialize(ApplicationDbContext context)
		{
			// Look for any records
			if (context.Employees.Any())
			{
				return;
			}

			/* Generate data in the momory */
			context.Employees.AddRange(
				new Employee("John", "Whyne", new DateTime(1991, 8, 7)) { Id = 1, Created = DateTime.UtcNow, Modified = DateTime.UtcNow },
				new Employee("Mathias", "Gernold", new DateTime(1997, 10, 12)) { Id = 2, Created = DateTime.UtcNow, Modified = DateTime.UtcNow },
				new Employee("Julia", "Reynolds", new DateTime(1955, 12, 16)) { Id = 3, Created = DateTime.UtcNow, Modified = DateTime.UtcNow },
				new Employee("Alois", "Mock", new DateTime(1935, 2, 9)) { Id = 4, Created = DateTime.UtcNow, Modified = DateTime.UtcNow },
				new Employee("Gertraud", "Bochold", new DateTime(2001, 3, 4)) { Id = 5, Created = DateTime.UtcNow, Modified = DateTime.UtcNow }
			);

			context.SaveChanges();
		}
	}
}
