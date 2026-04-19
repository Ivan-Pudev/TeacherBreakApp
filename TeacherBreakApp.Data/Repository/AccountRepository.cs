namespace TeacherBreakApp.Data.Repository
{
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;
    using Models;
    using Contracts;
    using Microsoft.EntityFrameworkCore;
    public class AccountRepository : BaseRepository,IAccountRepository
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public AccountRepository(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            TeacherBreakAppDbContext db) : base(db)
        {
            _userManager = userManager;
        }


        public async Task<IEnumerable<LeaveBalance>> GetLeaveBalancesAsync()
        {
            List<LeaveBalance> balances = await DbContext.LeaveBalances
                .Include(lb => lb.Teacher)
                .ToListAsync();

            return balances;
        }

        public async Task<LeaveBalance?> GetLeaveBalanceWithTeacherByIdAsync(Guid? id)
        {
            LeaveBalance? lb = await DbContext.LeaveBalances
                .Include(l => l.Teacher)
                .FirstOrDefaultAsync(l => l.Id == id);

            return lb;
        }

        public async Task<bool> AddLeaveBalanceAsync(LeaveBalance lb)
        {
            await DbContext.LeaveBalances.AddAsync(lb);
            int resultCount = await SaveChangesAsync();

            return resultCount > 0;
        }

        public async Task<bool> UpdateLeaveBalanceAsync(LeaveBalance lb)
        {
            DbContext.LeaveBalances.Update(lb);
            int resultCount = await SaveChangesAsync();

            return resultCount > 0;
        }

        public async Task<bool> RestoreQuizAsync(LeaveBalance lb)
        {
            lb.IsDeleted = false;
            int resultsCount = await SaveChangesAsync();

            return resultsCount > 0;
        }

        public async Task<bool> SoftDeleteLeaveBalanceAsync(LeaveBalance lb)
        {
            lb.IsDeleted = true;
            int resultCount = await SaveChangesAsync();

            return resultCount > 0;
        }

        public async Task<bool> HardDeleteLeaveBalanceAsync(LeaveBalance lb)
        {
            DbContext.LeaveBalances.Remove(lb);
            int resultCount = await SaveChangesAsync();

            return resultCount > 0;
        }
    }
}
