using System.Collections.Generic;

namespace EmployeesWebApiOData.IntegrationTests.Services
{
	public class ODataResponse<T>
	{
		public List<T> Value { get; set; }
	}
}
