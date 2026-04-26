using System.Security.Claims;
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

        private readonly IAccountRepository _accountRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(IAccountRepository accountRepository,
            UserManager<ApplicationUser> userManager)
        {
            _accountRepository = accountRepository;
            _userManager = userManager;
        }

        public async Task<Guid> IsUserValidAsync(ClaimsPrincipal user)
        {
            ApplicationUser? appUser = await _userManager.GetUserAsync(user);

            if (appUser == null)
            {
                throw new InvalidOperationException();
            }

            return appUser.Id;
        }

        public Task<IEnumerable<LeaveBalance>> GetLeaveBalancesAsync()
        {
            return _accountRepository.GetLeaveBalancesWithEntriesAsync();
        }

        public Task<LeaveBalance?> GetLeaveBalanceByIdAsync(Guid? id)
        {
            return _accountRepository.GetLeaveBalanceWithTeacherByIdAsync(id);
        }

        public Task<LeaveBalance?> GetLeaveBalanceWithTeacherIdAsync(Guid? id)
        {
            return _accountRepository.GetLeaveBalanceWithTeacherIdAsync(id);
        }

        public async Task<EditLeaveViewModel?> DisplayEdit(Guid? id)
        {
            LeaveBalance? lb = await GetLeaveBalanceByIdAsync(id);

            if (lb == null) throw new InvalidOperationException();

            var entriesByMonth = Enumerable.Range(0, 12)
                .Select(_ => new List<LeaveEntryViewModel>())
                .ToList();

            foreach (var entry in lb.LeaveEntries.Where(e => !e.IsDeleted))
            {
                int monthIndex = entry.StartDate.Month - 1;

                if (monthIndex < 0 || monthIndex > 11) continue;

                entriesByMonth[monthIndex].Add(new LeaveEntryViewModel
                {
                    Id = entry.Id,
                    StartDate = entry.StartDate,
                    EndDate = entry.EndDate,
                    IsDeleted = false
                });
            }

            return new EditLeaveViewModel
            {
                LeaveBalanceId = lb.Id,
                TeacherName = lb.Teacher.FullName,
                Year = lb.Year,
                CarryOverDays = lb.CarryOverDays,
                AnnualLeaveDays = lb.AnnualLeaveDays,
                AdditionalLeaveDays = lb.AdditionalLeaveDays,
                EntriesByMonth = entriesByMonth
            };
        }

        public async Task UpdateLeaveBalanceAsync(Guid id, EditLeaveViewModel vm)
        {
            LeaveBalance? lb = await GetLeaveBalanceByIdAsync(id);
            if (lb == null) throw new InvalidOperationException();

            lb.CarryOverDays = vm.CarryOverDays;
            lb.AnnualLeaveDays = vm.AnnualLeaveDays;
            lb.AdditionalLeaveDays = vm.AdditionalLeaveDays;

            var submittedEntries = vm.EntriesByMonth
                .Where(monthList => monthList != null)
                .SelectMany(monthList => monthList)
                .ToList();

            var submittedExistingIds = submittedEntries
                .Where(e => e.Id.HasValue && e.Id != Guid.Empty)
                .Select(e => e.Id!.Value)
                .ToHashSet();

            foreach (LeaveEntry dbEntry in lb.LeaveEntries.Where(e => !e.IsDeleted))
            {
                if (!submittedExistingIds.Contains(dbEntry.Id))
                {
                    bool isDeleteSuccessful = await _accountRepository.SoftDeleteLeaveEntryAsync(dbEntry);

                    if (!isDeleteSuccessful)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }

            foreach (LeaveEntryViewModel submitted in submittedEntries)
            {
                if (submitted.StartDate == default || submitted.EndDate == default)
                    continue;

                if (submitted.EndDate < submitted.StartDate)
                    continue;

                bool isNew = !submitted.Id.HasValue || submitted.Id == Guid.Empty;

                if (isNew)
                {
                    if (!submitted.IsDeleted)
                    {
                        var newEntry = new LeaveEntry
                        {
                            Id = Guid.NewGuid(),
                            LeaveBalanceId = lb.Id,
                            StartDate = submitted.StartDate,
                            EndDate = submitted.EndDate,
                            Days = CountWeekdays(submitted.StartDate, submitted.EndDate),
                            IsDeleted = false
                        };

                        bool isAddSuccessful = await _accountRepository.AddLeaveEntryAsync(newEntry);

                        lb.LeaveEntries.Add(newEntry);
                    }
                }
                else
                {
                    var existing = lb.LeaveEntries
                        .FirstOrDefault(e => e.Id == submitted.Id!.Value);

                    if (existing == null)
                        continue; 
                    if (submitted.IsDeleted)
                    {
                        existing.IsDeleted = true;
                    }
                    else
                    {
                        existing.StartDate = submitted.StartDate;
                        existing.EndDate = submitted.EndDate;
                        existing.Days = CountWeekdays(submitted.StartDate, submitted.EndDate);
                        existing.IsDeleted = false;
                    }
                }
            }

            bool isUpdateSuccessful = await _accountRepository.UpdateLeaveBalanceAsync(lb);

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

            var newBalance = new LeaveBalance()
            {
                TeacherId = user.Id,
                IsDeleted = false,
                Year = vm.Year,
                CarryOverDays = vm.CarryOverDays,
                AnnualLeaveDays = vm.AnnualLeaveDays,
                AdditionalLeaveDays = vm.AdditionalLeaveDays,
            };

            await _accountRepository.AddLeaveBalanceAsync(newBalance);
        }

        public async Task HardDeleteLeaveBalanceAsync(Guid id)
        {
            LeaveBalance? lb = await GetLeaveBalanceByIdAsync(id);

            if (lb == null)
                throw new InvalidOperationException();

            await _accountRepository.HardDeleteLeaveBalanceAsync(lb);

        }

        private static int CountWeekdays(DateOnly start, DateOnly end)
        {
            int count = 0;
            var current = start;
            while (current <= end)
            {
                var dow = current.DayOfWeek;
                if (dow != DayOfWeek.Saturday && dow != DayOfWeek.Sunday)
                    count++;
                current = current.AddDays(1);
            }
            return count;
        }
    }
}
