using TeacherBreakApp.Data.Models;
using TeacherBreakApp.Models;

namespace TeacherBreakApp.Services.Contracts
{
    using System.Collections.Generic;

    public interface IAccountService
    {
        Task<IEnumerable<LeaveBalance>> GetLeaveBalancesAsync();

        Task<LeaveBalance?> GetLeaveBalanceByIdAsync(Guid? id);

        Task<EditLeaveViewModel?> DisplayEdit(Guid? id);

        Task UpdateLeaveBalanceAsync(Guid id, EditLeaveViewModel vm);

        Task CreateUserAsync(CreateTeacherViewModel vm);

        Task HardDeleteLeaveBalanceAsync(Guid id);
    }
}
