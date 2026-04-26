using System.Security.Claims;
using TeacherBreakApp.Data.Models;

namespace TeacherBreakApp.Data.Repository.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Text;


    public interface IAccountRepository
    {
        Task<IEnumerable<LeaveBalance>> GetLeaveBalancesWithEntriesAsync();

        Task<LeaveBalance?> GetLeaveBalanceWithTeacherByIdAsync(Guid? id);

        Task<LeaveBalance?> GetLeaveBalanceWithTeacherIdAsync(Guid? id);

        Task<bool> AddLeaveBalanceAsync(LeaveBalance lb);

        Task<bool> AddLeaveEntryAsync(LeaveEntry le);

        Task<bool> UpdateLeaveBalanceAsync(LeaveBalance lb);

        Task<bool> UpdateLeaveEntryAsync(LeaveEntry le);

        Task<bool> RestoreLeaveBalanceAsync(LeaveBalance lb);

        Task<bool> SoftDeleteLeaveBalanceAsync(LeaveBalance lb);

        Task<bool> SoftDeleteLeaveEntryAsync(LeaveEntry le);

        Task<bool> HardDeleteLeaveBalanceAsync(LeaveBalance lb);

        Task<bool> HardDeleteLeaveEntryAsync(LeaveEntry le);
    }
}
