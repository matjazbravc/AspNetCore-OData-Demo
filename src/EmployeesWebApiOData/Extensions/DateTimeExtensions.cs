using System;

namespace EmployeesWebApiOData.Extensions
{
	public static class DateTimeExtensions
	{
		public static int CalculateAge(this DateTime birthDate)
		{
			var age = DateTime.Today.Year - birthDate.Year;
			if (DateTime.Today.Month < birthDate.Month ||
			    (DateTime.Today.Month == birthDate.Month && DateTime.Today.Day < birthDate.Day))
			{
				age--;
			}
			return age;
		}
	}
}