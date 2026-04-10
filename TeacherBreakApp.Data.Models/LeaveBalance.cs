using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeacherBreakApp.Data.Models
{
    public class LeaveBalance
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [ForeignKey(nameof(Teacher))]
        public Guid TeacherId { get; set; }

        public ApplicationUser Teacher { get; set; } = null!;

        public bool IsDeleted { get; set; }

        public int Year { get; set; }

        public int CarryOverDays { get; set; }
        
        public int AnnualLeaveDays { get; set; }   

        public int AdditionalLeaveDays { get; set; } 

        public ICollection<LeaveEntry> LeaveEntries { get; set; } 
            = new List<LeaveEntry>();
    }
}
