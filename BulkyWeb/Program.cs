using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb;

public class Program
{
	public static void Main(string[] args) {
		var builder = WebApplication.CreateBuilder(args);

		// --------------------------------------------------
		// Add services to the container.
		builder.Services.AddControllersWithViews();

		// Zelf toegevoegd
		// DbContext
		builder.Services.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

		// Scaffolding Identity - V.+/-8u15
		// > Default user toevoegen
		// > Optioneel confirmatieMail bij SignIn
		// > Mapping van IdentityTables met ApplicationDbContext
		builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
			.AddEntityFrameworkStores<ApplicationDbContext>();

		// > Toevoegen na Scaffolding Identity
		builder.Services.AddRazorPages();

		// # _categoryRepo
		// builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

		// # Unit of Work
		builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


		// --------------------------------------------------
		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (!app.Environment.IsDevelopment()) {
			app.UseExceptionHandler("/Home/Error");
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
		}

		app.UseHttpsRedirection();
		app.UseStaticFiles();

		app.UseRouting();

		// Scaffolding Identity - V.+/-8u15
		// > Manueel toevoegen van onderstaande
		// > Authentication komt altijd voor Authorization
		app.UseAuthentication();

		app.UseAuthorization();

		// > Toevoegen na Scaffolding Identity
		app.MapRazorPages();

		app.MapControllerRoute(
			name: "default",
			pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

		app.Run();
	}
}