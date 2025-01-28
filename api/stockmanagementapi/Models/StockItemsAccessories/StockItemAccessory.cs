using stockmanagementapi.Models.StockAccessories;
using stockmanagementapi.Models.StockItems;
using System.Text.Json.Serialization;

namespace stockmanagementapi.Models.StockItemsAccessories
{
  public class StockItemAccessory
  {
    /// <summary>
    /// Gets or sets the Stock Item Accessory Id.
    /// </summary>
    public int StockItemAccessoryId { get; set; }

    /// <summary>
    /// Gets or sets the stock item id.
    /// </summary>
    public int StockItemId { get; set; }

    [JsonIgnore]
    public StockItem? StockItem { get; set; }

    /// <summary>
    /// Gets or sets the stock accessory id.
    /// </summary>
    public int StockAccessoryId { get; set; }
    
    [JsonIgnore]    
    public StockAccessory? StockAccessory { get; set; }
  }
}
