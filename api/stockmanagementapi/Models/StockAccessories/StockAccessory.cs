using stockmanagementapi.Models.StockItemsAccessories;
using System.ComponentModel.DataAnnotations;

namespace stockmanagementapi.Models.StockAccessories
{
  public class StockAccessory
  {
    public int StockAccessoryId { get; set; }

    [StringLength(100)]
    public required string Name { get; set; }

    [StringLength(200)]
    public string Description { get; set; } = string.Empty;

    public List<StockItemAccessory>? StockItemAccessories { get; set; }

    public bool IsDeleted { get; set; }
  }
}
