using System;

namespace EmployeesWebApiOData.Models.Common
{
    public interface IBaseEntity
    {
		int Id { get; set; }

	    DateTime Created { get; set; }
		
	    DateTime Modified { get; set; }
    }
}