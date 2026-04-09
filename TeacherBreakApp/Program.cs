using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TeacherBreakApp.Data;
using TeacherBreakApp.Data.Contracts;
using TeacherBreakApp.Data.Models;
using TeacherBreakApp.Web.Infrastructure;

namespace TeacherBreakApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<TeacherBreakAppDbContext>(options =>
            options.UseSqlServer(
         builder.Configuration.GetConnectionString("DefaultConnection"),
         sql => sql.EnableRetryOnFailure()));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<TeacherBreakAppDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IIdentitySeeder, IdentitySeeder>();


            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<TeacherBreakAppDbContext>();
                await db.Database.MigrateAsync();
            }

            app.UseRolesSeeder();
            app.UseAdminUserSeeder();

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}");

            await app.RunAsync();
        }
    }
}
