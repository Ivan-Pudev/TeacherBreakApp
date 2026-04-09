namespace TeacherBreakApp.Models
{
    public class EditLeaveViewModel
    {
        public int LeaveBalanceId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public int TotalLeaveDays { get; set; }
        public int UsedLeaveDays { get; set; }
        public int TotalSickDays { get; set; }
        public int UsedSickDays { get; set; }
    }
}
