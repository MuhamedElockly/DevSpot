using DevSpot.Constants;
using DevSpot.Data;
using DevSpot.Models;
using DevSpot.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DevSpot
{
	public class Program
	{
		public static void Main(string[] args)
		{


			var builder = WebApplication.CreateBuilder(args);
			//  var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");
			builder.Services.AddDbContext<ApplicationDbContext>(
				options => options.UseSqlServer(
					builder.Configuration
					.GetConnectionString("Database"))
				);

			builder.Services.AddDefaultIdentity<IdentityUser>
				(options => options.SignIn.RequireConfirmedAccount = false)
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>();


			// Add services to the container.
			builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IRepository<JobPosting>, JobPostingRepository>();

            var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			using (var scope = app.Services.CreateScope())
			{
				var services = scope.ServiceProvider;

				RoleSeeder.SeedRoleAsync(services).Wait();
				UserSeeder.SeedUserAsync(services).Wait();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();
			app.MapRazorPages();
			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=JobPostings}/{action=Index}/{id?}");

			app.Run();
		}
	}
}
