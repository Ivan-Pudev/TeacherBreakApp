namespace TeacherBreakApp.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;
    using Data;
    using TeacherBreakApp.Data.Models;
    using Models;

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly TeacherBreakAppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public AdminController(
            TeacherBreakAppDbContext db,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Dashboard()
        {
            var balances = await _db.LeaveBalances
                .Include(lb => lb.Teacher)
                .ToListAsync();

            return View(balances);
        }

        [HttpGet]
        public async Task<IActionResult> EditLeave(Guid id)
        {
            var lb = await _db.LeaveBalances
                .Include(l => l.Teacher)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lb == null) return NotFound();

            return View(new EditLeaveViewModel
            {
                //LeaveBalanceId = lb.Id,
                //TeacherName = lb.Teacher?.UserName ?? "(unknown)",
                //TotalLeaveDays = lb.TotalLeaveDays,
                //UsedLeaveDays = lb.UsedLeaveDays,
                //TotalSickDays = lb.TotalSickDays,
                //UsedSickDays = lb.UsedSickDays,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLeave(EditLeaveViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var lb = await _db.LeaveBalances.FindAsync(model.LeaveBalanceId);
            if (lb == null) return NotFound();

            //lb.TotalLeaveDays = model.TotalLeaveDays;
            //lb.UsedLeaveDays = model.UsedLeaveDays;
            //lb.TotalSickDays = model.TotalSickDays;
            //lb.UsedSickDays = model.UsedSickDays;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Dashboard));
        }

        [HttpGet]
        public IActionResult CreateTeacher() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTeacher(CreateTeacherViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var existing = await _userManager.FindByEmailAsync(model.Email);
            if (existing != null)
            {
                ModelState.AddModelError("", "Email already exists.");
                return View(model);
            }

            var user = new ApplicationUser
            {
                FullName = model.FullName,
                Email = model.Email,
                UserName = model.Email,
                EmailConfirmed = true
            };

            var create = await _userManager.CreateAsync(user, model.Password);
            if (!create.Succeeded)
            {
                foreach (var e in create.Errors) ModelState.AddModelError("", e.Description);
                return View(model);
            }

            await _userManager.AddToRoleAsync(user, "Teacher");

            _db.LeaveBalances.Add(new LeaveBalance { TeacherId = user.Id });
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Dashboard));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLeave(Guid id)
        {
            var leaveBalance = await _db.LeaveBalances
                .FirstOrDefaultAsync(lb => lb.Id == id);

            if (leaveBalance == null)
                return NotFound();

            _db.LeaveBalances.Remove(leaveBalance);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Dashboard));
        }
    }
}