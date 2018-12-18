using EmployeesWebApiOData.DbContexts;
using EmployeesWebApiOData.Models;
using EmployeesWebApiOData.Services.Repositories.Common;
using Microsoft.Extensions.Logging;

namespace EmployeesWebApiOData.Services.Repositories
{
    public sealed class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext appDbContext, ILoggerFactory loggerFactory) 
	        : base(appDbContext, loggerFactory)
        {
        }
    }
}
