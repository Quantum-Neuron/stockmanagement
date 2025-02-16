using Microsoft.EntityFrameworkCore;
using stockmanagementapi.Enums;
using stockmanagementapi.Models;
using stockmanagementapi.Models.StockAccessories;
using stockmanagementapi.Models.StockItems;
using stockmanagementapi.Models.StockItemsAccessories;

public static class GenerateSeedData
{
  public static async Task SeedAsync(ModelDbContext dbContext)
  {
    if (!dbContext.StockAccessories.Any())
    {
      var stockAccessories = new List<StockAccessory>()
      {
        new StockAccessory { Name = "Spare Wheel", Description = "Mint condition spare wheel." },
        new StockAccessory { Name = "Heated Seats", Description = "Seats have built-in heating functionality." },
                new StockAccessory { Name = "Keyless Entry", Description = "Can enter the car without a key." }
            };

      dbContext.StockAccessories.AddRange(stockAccessories);
      await dbContext.SaveChangesAsync().ConfigureAwait(false);
    }

    if (!dbContext.StockItems.Any())
    {
      var stockItems = new List<StockItem>()
            {
                new StockItem
                {
                    Colour = ColourEnum.Black, CostPrice = 1000.10M, CreationDate = DateTime.Now,
                    KMS = 1000, LastModifiedDate = DateTime.Now, Model = "Corolla", Make = "Toyota",
                    Price = 10020.10M, RegistrationNumber = "ABC123", ModelYear = 2020, VIN = "1234567890"
                },
                new StockItem
                {
                    Colour = ColourEnum.Blue, CostPrice = 2000.20M, CreationDate = DateTime.Now,
                    KMS = 2000, LastModifiedDate = DateTime.Now, Model = "Civic", Make = "Honda",
                    Price = 20040.20M, RegistrationNumber = "DEF456", ModelYear = 2024, VIN = "0987654321"
                }
            };

      dbContext.StockItems.AddRange(stockItems);
      await dbContext.SaveChangesAsync().ConfigureAwait(false);
    }

    if (!dbContext.StockItemAccessories.Any())
    {
      var spareWheel = await dbContext.StockAccessories.FirstOrDefaultAsync(sa => sa.Name == "Spare Wheel");
      var heatedSeats = await dbContext.StockAccessories.FirstOrDefaultAsync(sa => sa.Name == "Heated Seats");
      var corolla = await dbContext.StockItems.FirstOrDefaultAsync(si => si.Model == "Corolla");
      var civic = await dbContext.StockItems.FirstOrDefaultAsync(si => si.Model == "Civic");

      if (spareWheel != null && corolla != null && heatedSeats != null && civic != null)
      {
        var stockItemAccessories = new List<StockItemAccessory>()
                {
                    new StockItemAccessory { StockItemId = corolla.StockItemId, StockAccessoryId = spareWheel.StockAccessoryId },
                    new StockItemAccessory { StockItemId = civic.StockItemId, StockAccessoryId = heatedSeats.StockAccessoryId }
                };

        dbContext.StockItemAccessories.AddRange(stockItemAccessories);
        await dbContext.SaveChangesAsync().ConfigureAwait(false);
      }
      else
      {
        Console.WriteLine("Could not seed data");
      }
    }
  }
}