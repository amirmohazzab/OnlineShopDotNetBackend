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

    public byte[] Thumbnail { get; set; }

    public string ThumbnailFileName { get; set; }

    public long ThumbnailFileSize { get; set; }

    public string ThumbnailFileExtension { get; set; }


}