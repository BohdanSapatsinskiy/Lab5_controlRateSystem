using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace controlRateSystem.Data.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введіть назву категорії")]
        [StringLength(100, ErrorMessage = "Назва категорії не може перевищувати 100 символів")]
        public string Name { get; set; } = string.Empty;

        public List<Currency> Currencies { get; set; } = new();
    }
}
