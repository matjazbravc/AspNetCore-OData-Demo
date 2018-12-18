using System;
using EmployeesWebApiOData.DbContexts;
using EmployeesWebApiOData.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EmployeesWebApiOData.IntegrationTests.Services
{
	public class TestWebApplicationFactory<TStartup> : WebApplicationFactory<Startup>
	{
		public ApplicationDbContext Context { get; private set; }

		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			builder.ConfigureServices(services =>
			{
				// Create a new service provider.
				var serviceProvider = new ServiceCollection()
					.AddEntityFrameworkInMemoryDatabase()
					.BuildServiceProvider();

				// Add a database context (AppDbContext) using an in-memory database for testing
				services.AddDbContext<ApplicationDbContext>(options =>
				{
					options.UseInMemoryDatabase("InMemoryDatabase");
					options.UseInternalServiceProvider(serviceProvider);
				});

				// Build the service provider.
				var sp = services.BuildServiceProvider();

				// Create a scope to obtain a reference to the database contexts
				var serviceScope = sp.CreateScope();
				var scopedServices = serviceScope.ServiceProvider;
				var appDb = scopedServices.GetRequiredService<ApplicationDbContext>();
				var logger = scopedServices.GetRequiredService<ILogger<TestWebApplicationFactory<TStartup>>>();
				
				// Set DbContext
				Context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

				// Ensure the database is created
				appDb.Database.EnsureCreated();

				try
				{
					// Seed the database with some test data
					SeedData.Initialize(Context);
				}
				catch (Exception ex)
				{
					logger.LogError(ex, "An error occurred seeding the database with test messages. Error: {ex.Message}");
				}
			});
		}
	}
}
