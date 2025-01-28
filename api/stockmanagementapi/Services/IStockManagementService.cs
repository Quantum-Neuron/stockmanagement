using Microsoft.AspNetCore.Mvc;
using stockmanagementapi.Models.StockItems;
using stockmanagementapi.Models.StockItems.Command;
using stockmanagementapi.Models.StockItems.Lookup;

namespace stockmanagementapi.Services
{
  public interface IStockManagementService
  {
    Task<List<StockItemLookup>> GetStockItemsAsync();

    Task<StockItemLookup> GetStockItemAsync();

    Task<IResult> DeleteStockItemAsync(int id);
    
    Task<IResult> CreateStockItemAsync(StockItemCommand item);

    Task<IResult> UpdateStockItemAsync(StockItemCommand stockItemCommand);
  }
}
