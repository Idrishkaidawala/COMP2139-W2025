using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartInventoryManagement.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity in stock cannot be negative")]
        public int QuantityInStock { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Low stock threshold cannot be negative")]
        public int LowStockThreshold { get; set; }

        // Foreign key
        public int CategoryId { get; set; }

        // Navigation property
        public Category? Category { get; set; }

        // Navigation property for orders
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}