using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeesWebApiOData.Models
{
	public class EmployeeMap : IEntityTypeConfiguration<Employee>
	{
		public void Configure(EntityTypeBuilder<Employee> builder)
		{
			builder.ToTable("Employees");
			builder.HasKey(m => m.Id);
			builder.Property(m => m.FirstName).IsRequired().HasMaxLength(128).IsUnicode();
			builder.Property(m => m.LastName).IsRequired().HasMaxLength(128).IsUnicode();
			builder.Property(m => m.BirthDate).IsRequired();
		}
	}
}
