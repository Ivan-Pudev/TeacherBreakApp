using TeacherBreakApp.Data.Models;

namespace TeacherBreakApp.Data.Repository.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Text;


    public interface IAdminRepository
    {
        Task<IEnumerable<LeaveBalance>> GetLeaveBalancesAsync();

        Task<LeaveBalance?> GetLeaveBalanceWithTeacherByIdAsync(Guid? id);

        Task<bool> AddLeaveBalanceAsync(LeaveBalance lb);

        Task<bool> UpdateLeaveBalanceAsync(LeaveBalance lb);

        Task<bool> RestoreQuizAsync(LeaveBalance lb);

        Task<bool> SoftDeleteLeaveBalanceAsync(LeaveBalance lb);

        Task<bool> HardDeleteLeaveBalanceAsync(LeaveBalance lb);

    }
}
