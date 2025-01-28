using stockmanagementapi.Enums;
using stockmanagementapi.Models.SoftDelete;
using stockmanagementapi.Models.StockItemsAccessories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stockmanagementapi.Models.StockItems
{
  public class StockItem : ISoftDeleteOn
  {
    [Key]
    public int StockItemId { get; set; }

    /// <summary>
    /// Gets or sets the registration number of the stock item.
    /// </summary>
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

    public List<StockItemAccessory>? StockItemAccessories { get; set; }

    /// <summary>
    /// Gets or sets the record state whether the record is deleted or not.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets the creation date of the record.
    /// </summary>
    public DateTime CreationDate { get; set; }

    /// <summary>
    /// Gets or sets the last time the record was modified.
    /// </summary>
    public DateTime LastModifiedDate { get; set; }
  }
}
