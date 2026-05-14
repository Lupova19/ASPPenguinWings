using System.ComponentModel.DataAnnotations;

namespace ASPPenguinWings.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Въведете име")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Изберете категория")]
        public int CategoryId { get; set; }
        public Category Categories { get; set; }
        [Required(ErrorMessage = "Въведете размер")]
        public string Size { get; set; }
        [Required(ErrorMessage = "Въведете количество")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Въведете описание")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Въведете за кого е продукта")]
        public string Apply { get; set; }
        [Required(ErrorMessage = "Добавете снимка")]
        public string ImageUrl { get; set; }
        [Required(ErrorMessage = "Въведете цена")]
        public decimal Price { get; set; }
        public DateTime DateOn { get; set; }
        public ICollection<Order> Orders { get; set; }

    }
}
