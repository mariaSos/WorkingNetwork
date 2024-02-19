using System.ComponentModel.DataAnnotations;

namespace ApiAppLesson3.Contracts.Responses
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; } = decimal.Zero;
        public int? CategoryId { get; set; }

    }
}
