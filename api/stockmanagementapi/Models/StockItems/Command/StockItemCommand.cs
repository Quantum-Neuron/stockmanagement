using stockmanagementapi.Enums;
using stockmanagementapi.Models.StockAccessories;
using stockmanagementapi.Models.StockItemsAccessories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stockmanagementapi.Models.StockItems.Command
{
  public class StockItemCommand
  {
    public int StockItemId { get; set; }

    [StringLength(50)]
    public required string RegistrationNumber { get; set; }

    [StringLength(50)]
    public required string Make { get; set; }

    [StringLength(50)]
    public required string Model { get; set; }

    [Range(1900, 2100)]
    public int ModelYear { get; set; }

    public int KMS { get; set; }

    public required ColourEnum Colour { get; set; }

    [StringLength(100)]
    public required string VIN { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public required decimal Price { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public required decimal CostPrice { get; set; }
    
    public List<StockItemAccessory> StockItemAccessories { get; set; } = new List<StockItemAccessory>();

    public IFormFile? PrimaryImage { get; set; }

    public List<IFormFile>? SecondaryImages { get; set; }

  }
}
