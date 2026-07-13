using Microsoft.EntityFrameworkCore.Metadata;
using TeacherBreakApp.Services.Contracts;

namespace TeacherBreakApp.Controllers
{
    using Data;
    using TeacherBreakApp.Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

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

                var balance = await _accountService.GetLeaveBalanceByIdAsync(userId);

                if (balance == null)
                {
                    ViewBag.Message = "Вашият профил все още не е конфигуриран от администратор.";
                    return View();
                }

                return View(balance);
            }
            catch (Exception)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity", });
            }
        }
    }
}
