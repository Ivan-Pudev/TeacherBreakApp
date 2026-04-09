namespace TeacherBreakApp.Controllers
{
    using TeacherBreakApp.Data;
    using TeacherBreakApp.Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        private readonly TeacherBreakAppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public TeacherController(UserManager<ApplicationUser> userManager, TeacherBreakAppDbContext db)
        {
            _userManager = userManager; 
            _db = db;
        }
        public async Task<IActionResult> Dashboard()
        {
            var userId = await _userManager.GetUserAsync(User);
            if (userId == null) return RedirectToAction("Login", "Account");
            var balance = await _db.LeaveBalances
                             .Include(lb => lb.Teacher)
                             .FirstOrDefaultAsync(lb => lb.TeacherId == userId.Id);

            if (balance == null)
            {
                ViewBag.Message = "Вашият профил все още не е конфигуриран от администратор.";
                return View();
            }

            return View(balance);
        }
    }
}
