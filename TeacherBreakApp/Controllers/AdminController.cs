using TeacherBreakApp.Services.Contracts;

namespace TeacherBreakApp.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Models;

    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        private readonly IAccountService _accountService;
        public AdminController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var balances = await _accountService.GetLeaveBalancesAsync();

            return View(balances);
        }

        [HttpGet]
        public async Task<IActionResult> EditLeave(Guid? id)
        {
            var lbViewModel = await _accountService.DisplayEdit(id);

            return View(lbViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLeave(Guid id,EditLeaveViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _accountService.UpdateLeaveBalanceAsync(Guid.Parse(model.LeaveBalanceId.ToString()), model);
            return RedirectToAction(nameof(Dashboard));
        }

        [HttpGet]
        public IActionResult CreateTeacher()
        {
           return View(new CreateTeacherViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTeacher(CreateTeacherViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _accountService.CreateUserAsync(model);
            
            return RedirectToAction(nameof(Dashboard));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLeave(Guid id)
        {
            await _accountService.HardDeleteLeaveBalanceAsync(id);

            return RedirectToAction(nameof(Dashboard));
        }
    }
}