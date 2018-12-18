using Microsoft.AspNet.OData.Builder;
using Microsoft.OData.Edm;

namespace EmployeesWebApiOData.Models
{
    public static class EdmModelBuilder
    {
        public static IEdmModel GetEdmModel()
        {
	        var builder = new ODataConventionModelBuilder();
	        builder.EntitySet<Employee>("Employees");
	        builder.Namespace = typeof(Employee).Namespace;
	        return builder.GetEdmModel();
        }
    }
}
