using System.ComponentModel.DataAnnotations;

namespace TeacherBreakApp.Models
{
    public class CreateTeacherViewModel
    {
        [Required(ErrorMessage = "Пълното име е задължително.")]
        [Display(Name = "Пълно име")]
        public string FullName { get; set; } = "";

        [Required(ErrorMessage = "Имейлът е задължителен.")]
        [EmailAddress(ErrorMessage = "Невалиден имейл адрес.")]
        [Display(Name = "Имейл адрес")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Паролата е задължителна.")]
        [MinLength(6, ErrorMessage = "Паролата трябва да е поне 6 символа.")]
        [Display(Name = "Временна парола")]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Годината е задължителна.")]
        [Display(Name = "Година")]
        public int Year { get; set; } = DateTime.Now.Year;

        [Required]
        [Range(0, 365, ErrorMessage = "Невалидна стойност.")]
        [Display(Name = "Остатък от минала година (пренесени дни)")]
        public int CarryOverDays { get; set; } = 0;

        [Required]
        [Range(0, 365, ErrorMessage = "Невалидна стойност.")]
        [Display(Name = "Полагаем отпуск за годината")]
        public int AnnualLeaveDays { get; set; } = 20;

        [Required]
        [Range(0, 365, ErrorMessage = "Невалидна стойност.")]
        [Display(Name = "Допълнителен отпуск")]
        public int AdditionalLeaveDays { get; set; } = 0;
    }
}