#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;

namespace ProductsAndCategories.Models;

public class Product
{
    [Key]
    public int ProductId { get; set; }

    [Required]
    [MaxLength(45)]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    [DataType(DataType.Currency)]
    public double? Price { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public List<Association> Categories { get; set; } = new();
}