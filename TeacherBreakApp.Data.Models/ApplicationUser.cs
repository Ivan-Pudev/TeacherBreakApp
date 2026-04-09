namespace TeacherBreakApp.Data.Models
{
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FullName { get; set; } = "";

        public bool IsDeleted { get; set; }

    }
}
