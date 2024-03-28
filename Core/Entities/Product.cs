using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

[Table("PProducts", Schema = "Base")]
public class Product 
{
    [Key]
    public int Id { get; set; }

    [MaxLength(128), Required]
    public string ProductName { get; set; }

    public int Price { get; set; }


}