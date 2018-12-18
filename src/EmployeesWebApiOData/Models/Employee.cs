using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EmployeesWebApiOData.Extensions;
using EmployeesWebApiOData.Models.Common;

namespace EmployeesWebApiOData.Models
{
	public class Employee : BaseEntity
    {
	    /* Required be EF */
	    public Employee()
	    {
	    }

	    public Employee(string firstName, string lastName, DateTime birthDate)
		{
			FirstName = firstName;
			LastName = lastName;
			BirthDate = birthDate;
			Age = birthDate.CalculateAge();
		}

	    [Required]
	    [MaxLength(128)]
	    [Display(Name = "FirstName", Description="Employee first name")]
		public string FirstName { get; set; } 

	    [Required]
	    [MaxLength(128)]
	    [Display(Name = "LastName", Description="Employee last name")]
	    public string LastName { get; set; }
	    
	    [Required]
	    [Display(Name = "BirthDate", Description="Employee Birth date")]
	    public DateTime BirthDate { get; set; }
	    
	    [Required]
	    [Display(Name = "Age", Description="Employee Age")]
	    public int Age { get; set; }
    }
}
