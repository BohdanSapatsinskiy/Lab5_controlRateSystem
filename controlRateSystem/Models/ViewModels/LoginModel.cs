using System.ComponentModel.DataAnnotations;

namespace controlRateSystem.Models.ViewModels
{
    public class LoginModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public string? ReturnUrl { get; set; }
    }
}
