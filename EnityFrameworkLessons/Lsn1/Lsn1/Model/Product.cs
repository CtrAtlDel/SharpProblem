using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Lsn1.Model;

public class Product
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }
    
    [Required]
    public decimal Price { get; set; }
    
}