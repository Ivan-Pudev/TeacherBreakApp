using TeacherBreakApp.Data.Models;
using TeacherBreakApp.Data.Repository.Contracts;
using TeacherBreakApp.Models;
using TeacherBreakApp.Services.Contracts;

namespace TeacherBreakApp.Services
{
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;

    public class AccountService : IAccountService
    {

        private readonly IAccountRepository _adminRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(IAccountRepository adminRepository,
            UserManager<ApplicationUser> userManager)
        {
            _adminRepository = adminRepository;
            _userManager = userManager;
        }

        public Task<IEnumerable<LeaveBalance>> GetLeaveBalancesAsync()
        {
            return _adminRepository.GetLeaveBalancesAsync();
        }

        public Task<LeaveBalance?> GetLeaveBalanceByIdAsync(Guid? id)
        {
            return _adminRepository.GetLeaveBalanceWithTeacherByIdAsync(id);
        }

        public async Task<EditLeaveViewModel?> DisplayEdit(Guid? id)
        {
            LeaveBalance? lb = await GetLeaveBalanceByIdAsync(id);

            if (lb == null) throw new InvalidOperationException();

            return (new EditLeaveViewModel
            {
                //LeaveBalanceId = lb.Id,
                //TeacherName = lb.Teacher?.UserName ?? "(unknown)",
                //TotalLeaveDays = lb.TotalLeaveDays,
                //UsedLeaveDays = lb.UsedLeaveDays,
                //TotalSickDays = lb.TotalSickDays,
                //UsedSickDays = lb.UsedSickDays,
            });
        }

        public async Task UpdateLeaveBalanceAsync(Guid id, EditLeaveViewModel vm)
        {
            var lb = await GetLeaveBalanceByIdAsync(id);
            if (lb == null) throw new InvalidOperationException();

            //lb.TotalLeaveDays = model.TotalLeaveDays;
            //lb.UsedLeaveDays = model.UsedLeaveDays;
            //lb.TotalSickDays = model.TotalSickDays;
            //lb.UsedSickDays = model.UsedSickDays;

            bool isUpdateSuccessful = await _adminRepository.UpdateLeaveBalanceAsync(lb);

            if (!isUpdateSuccessful)
            {
                throw new InvalidOperationException();
            }
        }

        public async Task CreateUserAsync(CreateTeacherViewModel vm)
        {
            ApplicationUser? existing = await _userManager.FindByEmailAsync(vm.Email);
            if (existing != null)
            {
                throw new InvalidOperationException("Email already exists.");
            }

            var user = new ApplicationUser
            {
                FullName = vm.FullName,
                Email = vm.Email,
                UserName = vm.Email,
                EmailConfirmed = true,
                IsDeleted = false
            };

            var create = await _userManager.CreateAsync(user, vm.Password);
            if (!create.Succeeded)
            {
                foreach (var e in create.Errors) 
                    throw new InvalidOperationException(e.Description);
            }

            await _userManager.AddToRoleAsync(user, "Teacher");

            await _adminRepository.AddLeaveBalanceAsync(new LeaveBalance { TeacherId = user.Id });
        }

        public async Task HardDeleteLeaveBalanceAsync(Guid id)
        {
            LeaveBalance? lb = await GetLeaveBalanceByIdAsync(id);

            if (lb == null)
                throw new InvalidOperationException();

            await _adminRepository.HardDeleteLeaveBalanceAsync(lb);

        }
    }
}
