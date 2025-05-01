using System.ComponentModel.DataAnnotations;

namespace controlRateSystem.Models.ViewModels
{
    public class UserProfileModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Електронна пошта")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Новий пароль")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Підтвердження паролю")]
        [Compare("NewPassword", ErrorMessage = "Паролі не співпадають.")]
        public string? ConfirmPassword { get; set; }
    }
}
