using TeacherBreakApp.Data.Models;

namespace TeacherBreakApp.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Contracts;

    [Authorize(Roles = "Teacher")]
    public class TeacherController : BaseController
    {
        private readonly IAccountService _accountService;
        public TeacherController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                Guid userId = await _accountService.IsUserValidAsync(User);

                LeaveBalance? balance = await _accountService.GetLeaveBalanceWithTeacherIdAsync(userId);

                if (balance == null)
                {
                    ModelState.AddModelError("", "Вашият профил все още не е конфигуриран от администратор.");
                    return RedirectToPage("/Account/Login", new { area = "Identity", });
                }

                return View(balance);
            }
            catch (Exception e)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity", });
            }
        }
    }
}
