using stockmanagementapi.Models.StockAccessories;

namespace stockmanagementapi.Models
{
  public static class GenerateSeedData
  {
    public static async Task SeedAsync(ModelDbContext dbContext)
    {
      if (!dbContext.StockAccessories.Any())
      {
        var stockAccessories = new List<StockAccessory>()
        {
          new StockAccessory
          {
            Name = "Spare Wheel",
            Description = "Mint condition spare wheel."
          },
          new StockAccessory
          {
            Name = "Heated Seats",
            Description = "Seats have built-in heating functionality."
          },
          new StockAccessory
          {
            Name = "Keyless Entry",
            Description = "Can enter the car without a key."
          }
        };

        dbContext.StockAccessories.AddRange(stockAccessories);
        await dbContext.SaveChangesAsync().ConfigureAwait(false);
      }
    }
  }
}
