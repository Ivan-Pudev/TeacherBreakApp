using System.ComponentModel.DataAnnotations;

namespace TeacherBreakApp.Models
{
    public class LeaveEntryViewModel
    {
        public Guid? Id { get; set; }

        [Required] public DateOnly StartDate { get; set; }

        [Required] public DateOnly EndDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}