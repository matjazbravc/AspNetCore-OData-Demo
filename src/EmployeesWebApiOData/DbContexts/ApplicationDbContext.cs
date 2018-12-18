using System;
using System.Linq;
using System.Threading.Tasks;
using EmployeesWebApiOData.Models;
using EmployeesWebApiOData.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace EmployeesWebApiOData.DbContexts
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<Employee> Employees { get; set; }

		public override int SaveChanges()
		{
			UpdateEntitiesInfo();
			return base.SaveChanges();
		}

		public async Task<int> SaveChangesAsync()
		{
			UpdateEntitiesInfo();
			return await base.SaveChangesAsync();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.ApplyConfiguration(new EmployeeMap());
		}

		private void UpdateEntitiesInfo()
		{
			var entries = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
			foreach (var entry in entries)
			{
				if (entry.State == EntityState.Added)
				{
					((BaseEntity)entry.Entity).Created = DateTime.UtcNow;
				}
				((BaseEntity)entry.Entity).Modified = DateTime.UtcNow;
			}
		}
	}
}