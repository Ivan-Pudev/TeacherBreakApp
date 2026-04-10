using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TeacherBreakApp.Data.Models;

namespace TeacherBreakApp.Data
{
    public class TeacherBreakAppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid> 
    {
        public TeacherBreakAppDbContext(DbContextOptions<TeacherBreakAppDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<LeaveBalance> LeaveBalances { get; set; } = null!;

        public virtual DbSet<LeaveEntry> LeaveEntries { get; set; } = null!;

        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
