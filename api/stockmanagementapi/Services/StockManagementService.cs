using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stockmanagementapi.Models;
using stockmanagementapi.Models.StockItems;
using stockmanagementapi.Models.StockItems.Command;
using stockmanagementapi.Models.StockItems.Lookup;
using stockmanagementapi.Models.StockItemsAccessories;

namespace stockmanagementapi.Services
{
  public class StockManagementService : IStockManagementService
  {
    private readonly ModelDbContext dbContext;
    private readonly IMapper mapper;
    private readonly ILogger<StockManagementService> logger;

    public StockManagementService(
      ModelDbContext dbContext,
      IMapper mapper,
      ILogger<StockManagementService> logger
    )
    {
      this.dbContext = dbContext;
      this.mapper = mapper;
      this.logger = logger;
    }

    public async Task<IResult> CreateStockItemAsync(StockItemCommand item)
    {
      try
      {
        logger.LogInformation("Creating stock item.");

        var mappedStockItem = mapper.Map<StockItemCommand, StockItem>(item);

        mappedStockItem.CreationDate = DateTime.Now;
        mappedStockItem.LastModifiedDate = DateTime.Now;

        await dbContext.StockItems.AddAsync(mappedStockItem).ConfigureAwait(false);
        await dbContext.SaveChangesAsync().ConfigureAwait(false);

        foreach (var sia in item.StockItemAccessories)
        {
          var exists = await dbContext.StockItemAccessories
              .AnyAsync(s => s.StockItemId == mappedStockItem.StockItemId && s.StockAccessoryId == sia.StockAccessoryId)
              .ConfigureAwait(false);

          if (!exists)
          {
            var stockItemAccessory = new StockItemAccessory
            {
              StockItemId = mappedStockItem.StockItemId,
              StockAccessoryId = sia.StockAccessoryId
            };
            await dbContext.StockItemAccessories.AddAsync(stockItemAccessory).ConfigureAwait(false);
          }
        }

        await dbContext.SaveChangesAsync().ConfigureAwait(false);
        return Results.Ok("Successfully created stock item.");
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "Error creating stock item");
        return Results.BadRequest("Error creating stock item");
      }
    }

    public async Task<IResult> DeleteStockItemAsync(int id)
    {
      try
      {
        logger.LogInformation("Deleting stock item.");

        var stockItem = await dbContext.StockItems.FirstOrDefaultAsync(
          st => st.StockItemId == id
        ) ?? throw new InvalidOperationException("Stock item not found.");

        dbContext.StockItems.Remove(stockItem);
        await dbContext.SaveChangesAsync().ConfigureAwait(false);

        return Results.Ok("Successfully deleted stock item.");
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "Error deleting stock item");
        return Results.BadRequest("Error deleting stock item.");
      }
    }

    public Task<StockItemLookup> GetStockItemAsync()
    {
      throw new NotImplementedException();
    }

    public async Task<List<StockItemLookup>> GetStockItemsAsync()
    {
      try
      {
        var stockItems = await dbContext.StockItems.ToListAsync().ConfigureAwait(false);
        var mappedStockItems = mapper.Map<List<StockItemLookup>>(stockItems);

        return mappedStockItems;
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "Error getting stock items");
        throw new InvalidOperationException("Failed to get stock items.");
      }
    }

    public async Task<IResult> UpdateStockItemAsync(StockItemCommand stockItemCommand)
    {
      try
      {
        var stockItem = await dbContext.StockItems.FirstOrDefaultAsync(
          st => st.StockItemId == stockItemCommand.StockItemId
        ) ?? throw new InvalidOperationException("Stock item not found.");

        var mappedStockItems = mapper.Map(stockItemCommand, stockItem);

        mappedStockItems.LastModifiedDate = DateTime.Now;

        dbContext.StockItems.Update(mappedStockItems);
        await dbContext.SaveChangesAsync().ConfigureAwait(false);

        var existingAccessories = dbContext.StockItemAccessories.Where(sia => sia.StockItemId == mappedStockItems.StockItemId);
        dbContext.StockItemAccessories.RemoveRange(existingAccessories);
        await dbContext.SaveChangesAsync().ConfigureAwait(false);

        foreach (var accessory in stockItemCommand.StockItemAccessories)
        {
          var exists = await dbContext.StockItemAccessories
              .AnyAsync(s => s.StockItemId == mappedStockItems.StockItemId && s.StockAccessoryId == accessory.StockAccessoryId)
              .ConfigureAwait(false);

          if (!exists)
          {
            var stockItemAccessory = new StockItemAccessory
            {
              StockItemId = mappedStockItems.StockItemId,
              StockAccessoryId = accessory.StockAccessoryId
            };
            dbContext.StockItemAccessories.Add(stockItemAccessory);
          }
        }

        await dbContext.SaveChangesAsync().ConfigureAwait(false);
        return Results.Ok("Successfully updated stock item.");
      }
      catch(Exception ex)
      {
        logger.LogError(ex, "Error updating stock item");
        return Results.BadRequest("Error updating stock item.");
      }
    }
  }
}
