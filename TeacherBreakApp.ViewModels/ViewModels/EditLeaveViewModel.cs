using System.ComponentModel.DataAnnotations;

namespace TeacherBreakApp.Models
{
    public class EditLeaveViewModel
    {
        public Guid LeaveBalanceId { get; set; }

        public string TeacherName { get; set; } = "";

        public int Year { get; set; }

        [Range(0, 365)]
        public int CarryOverDays { get; set; }

        [Range(0, 365)]
        public int AnnualLeaveDays { get; set; }

        [Range(0, 365)]
        public int AdditionalLeaveDays { get; set; }

        public List<List<LeaveEntryViewModel>> EntriesByMonth { get; set; }
            = Enumerable.Range(0, 12)
                .Select(_ => new List<LeaveEntryViewModel>())
                .ToList();
    }
}
