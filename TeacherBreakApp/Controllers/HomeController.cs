using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TeacherBreakApp.Models;

namespace TeacherBreakApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                if (User.IsInRole("Admin"))
                    return RedirectToAction("Dashboard", "Admin");

                if (User.IsInRole("Teacher"))
                    return RedirectToAction("Dashboard", "Teacher");
            }

            return RedirectToPage("/Account/Login", new { area = "Identity", });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
