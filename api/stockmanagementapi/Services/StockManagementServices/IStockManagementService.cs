using stockmanagementapi.Models.StockItemImages.Commands;
using stockmanagementapi.Models.StockItemImages.Lookups;
using stockmanagementapi.Models.StockItems;
using stockmanagementapi.Models.StockItems.Command;
using stockmanagementapi.Models.StockItems.Lookup;

namespace stockmanagementapi.Services.StockManagementServices
{
	public interface IStockManagementService
	{
		Task<PagedStockItems<StockItemLookup>> GetStockItemsAsync(int pageNumber, int pageSize);

		Task<StockItemLookup> GetStockItemAsync();

		Task<IResult> DeleteStockItemAsync(int id);

		Task<IResult> CreateStockItemAsync(StockItemCommand item);

		Task<IResult> UpdateStockItemAsync(StockItemCommand stockItemCommand);

		Task<List<StockImageLookup>> GetStockImagesAsync(int stockItemId);

		Task<List<StockImageLookup>> AddStockImagesAsync(List<StockImageCommand> stockImageCommands);

		Task<IResult> DeleteStockImageAsync(int id);

		Task<StockImageLookup> UpdateStockImageAsync(StockImageCommand stockImageCommand);
	}
}
