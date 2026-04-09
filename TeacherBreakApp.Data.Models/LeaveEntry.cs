using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeacherBreakApp.Data.Models
{
    using System;
    public class LeaveEntry
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [ForeignKey(nameof(LeaveBalance))]
        public Guid LeaveBalanceId { get; set; }

        public LeaveBalance LeaveBalance { get; set; } = null!;

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public int Days { get; set; }
    }
}
