using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace controlRateSystem.Data.Models
{
    public class Currency
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введіть назву валюти")]
        [StringLength(100, ErrorMessage = "Назва валюти не може перевищувати 100 символів")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть код валюти")]
        [StringLength(10, ErrorMessage = "Код валюти не може перевищувати 10 символів")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть курс долара")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "Ціна має бути більшою за 0")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal PriceInDollar { get; set; }

        [Required(ErrorMessage = "Оберіть категорію")]
        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;
    }
}
