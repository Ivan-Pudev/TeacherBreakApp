namespace TeacherBreakApp.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using Models;
    using Contracts;
    using Microsoft.EntityFrameworkCore;
    public class AccountRepository : BaseRepository,IAccountRepository
    {

        public AccountRepository(TeacherBreakAppDbContext db) : base(db) { }

        public async Task<IEnumerable<LeaveBalance>> GetLeaveBalancesWithEntriesAsync()
        {
            List<LeaveBalance> balances = await DbContext.LeaveBalances
                .Include(lb => lb.Teacher)
                .Include(lb=>lb.LeaveEntries)
                .Where(lb=>!lb.IsDeleted && lb.Year == DateTime.Now.Year)
                .ToListAsync();

            return balances;
        }

        public async Task<LeaveBalance?> GetLeaveBalanceWithTeacherByIdAsync(Guid? id)
        {
            LeaveBalance? lb = await DbContext.LeaveBalances
                .Include(l => l.Teacher)
                .Include(l=>l.LeaveEntries)
                .Where(lb=>!lb.IsDeleted && lb.Year == DateTime.Now.Year)
                .FirstOrDefaultAsync(l => l.Id == id);

            return lb;
        }

        public async Task<bool> AddLeaveBalanceAsync(LeaveBalance lb)
        {
            await DbContext.LeaveBalances.AddAsync(lb);
            int resultCount = await SaveChangesAsync();

            return resultCount > 0;
        }

        public async Task<bool> AddLeaveEntryAsync(LeaveEntry le)
        {
            await DbContext.LeaveEntries.AddAsync(le);
            int resultCount = await SaveChangesAsync();

            return resultCount > 0;
        }

        public async Task<bool> UpdateLeaveBalanceAsync(LeaveBalance lb)
        {
            DbContext.LeaveBalances.Update(lb);
            int resultCount = await SaveChangesAsync();

            return resultCount > 0;
        }

        public async Task<bool> UpdateLeaveEntryAsync(LeaveEntry le)
        {
            DbContext.LeaveEntries.Update(le);
            int resultCount = await SaveChangesAsync();

            return resultCount > 0;
        }

        public async Task<bool> RestoreLeaveBalanceAsync(LeaveBalance lb)
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

        public async Task<bool> SoftDeleteLeaveEntryAsync(LeaveEntry le)
        {
            le.IsDeleted = true;
            int resultCount = await SaveChangesAsync();

            return resultCount > 0;
        }

        public async Task<bool> HardDeleteLeaveEntryAsync(LeaveEntry le)
        {
            DbContext.LeaveEntries.Remove(le);
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
