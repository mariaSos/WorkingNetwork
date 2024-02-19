using System.ComponentModel.DataAnnotations;

namespace ApiAppLesson3.Models
{
    public class Product
    {
        [Key] public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; } = decimal.Zero;
        public int? CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public virtual ICollection<Store> Stores { get; set; } = new List<Store>();
    }
}
