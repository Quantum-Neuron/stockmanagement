namespace stockmanagementapi.Models.StockItems.Lookup
{
  public class StockItemLookup
  {
    public int StockItemId { get; set; }

    public string Colour { get; set; } = string.Empty;

    public string Make { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public string VINNumber { get; set; } = string.Empty;

    public string RegistrationNumber { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public decimal CostPrice { get; set; }

    public int KMS { get; set; }

    public int ModelYear { get; set; }
  }
}
