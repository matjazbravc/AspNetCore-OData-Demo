using EmployeesWebApiOData.DbContexts;
using EmployeesWebApiOData.Models;
using EmployeesWebApiOData.Services;
using EmployeesWebApiOData.Services.Repositories;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeesWebApiOData
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("EmloyeesDb"));

			// Register services
			services.AddScoped<IEmployeeRepository, EmployeeRepository>();

			services.AddOData();
			services.AddMvc(options => 
			{
				// https://github.com/Microsoft/aspnet-api-versioning/issues/361
				// Because conflicts with ODataRouting as of this version could improve performance though
				options.EnableEndpointRouting = false;
			}).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
			{
				var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
				SeedData.Initialize(context);
			}

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseMvc(b =>
			{
				b.Select().Expand().Filter().OrderBy().MaxTop(100).Count();
				b.MapODataServiceRoute("Employees", "odata", EdmModelBuilder.GetEdmModel());
			});
		}
	}
}
