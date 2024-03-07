using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AutomationSystem.Data;
using AutomationSystem.Areas.Identity.Data;

namespace AutomationSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("AutomationSystemContextConnection") ?? throw new InvalidOperationException("Connection string 'AutomationSystemContextConnection' not found.");

            builder.Services.AddDbContext<AutomationSystemContext>(options => options.UseSqlite(connectionString));

            builder.Services.AddDefaultIdentity<AutomationSystemUser>(options => options.SignIn.RequireConfirmedAccount = false).AddRoles<IdentityRole>().AddEntityFrameworkStores<AutomationSystemContext>();

            builder.Services.AddRazorPages();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            app.MapRazorPages();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Manager", "Client", "Master" };

                foreach (var role in roles)
                {

                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));

                }
            }

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            app.Run();
        }
    }
}