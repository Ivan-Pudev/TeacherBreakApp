using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using TeacherBreakApp.Data;
using TeacherBreakApp.Data.Contracts;
using TeacherBreakApp.Data.Models;
using TeacherBreakApp.Data.Repository;
using TeacherBreakApp.Data.Repository.Contracts;
using TeacherBreakApp.Services;
using TeacherBreakApp.Services.Contracts;
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

            builder.Services
                .AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                })
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<TeacherBreakAppDbContext>();

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.Filters.Add(new AuthorizeFilter(policy));
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";

                options.Cookie.Name = "TeacherBreakApp.Auth";
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;

                // session cookie
                options.Cookie.MaxAge = null;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(15);

                options.SlidingExpiration = false;
            });

            builder.Services.AddRazorPages();

            builder.Services.AddScoped<IIdentitySeeder, IdentitySeeder>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });

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

            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            await app.RunAsync();
        }
    }
}