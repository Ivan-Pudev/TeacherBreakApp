namespace TeacherBreakApp.Models
{
    using System.ComponentModel.DataAnnotations;

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Въведете имейл")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Въведете парола")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
